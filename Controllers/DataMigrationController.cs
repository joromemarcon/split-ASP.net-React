using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using split_api.Data;
using split_api.Models;

namespace split_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DataMigrationController : ControllerBase
    {
        private readonly DataContext _context;
        private readonly UserManager<SplitUser> _userManager;

        public DataMigrationController(DataContext context, UserManager<SplitUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [HttpGet("check-user-data/{username}")]
        public async Task<IActionResult> CheckUserData(string username)
        {
            try
            {
                var user = await _userManager.Users.FirstOrDefaultAsync(u => u.UserName == username);
                if (user == null)
                {
                    return NotFound($"User '{username}' not found");
                }

                var customerReceipts = await _context.CustomerReceipts
                    .Include(cr => cr.Receipt)
                    .ThenInclude(r => r.Items)
                    .Where(cr => cr.UserId == user.Id)
                    .ToListAsync();

                var result = new
                {
                    User = new { user.Id, user.UserName, user.Email, user.FullName },
                    ReceiptsCount = customerReceipts.Count,
                    Receipts = customerReceipts.Select(cr => new
                    {
                        ReceiptId = cr.ReceiptId,
                        ReceiptCode = cr.Receipt?.ReceiptCode,
                        EstablishmentName = cr.Receipt?.EstablishmentName,
                        TransactionTotal = cr.Receipt?.TransactionTotal,
                        ItemsCount = cr.Receipt?.Items?.Count ?? 0,
                        IsOwner = cr.isOwner,
                        IsPaid = cr.IsPaid,
                        Items = cr.Receipt?.Items?.Select(i => new
                        {
                            i.Id,
                            i.ItemName,
                            i.ItemPrice,
                            i.PaidCustomerId
                        })
                    })
                };

                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error checking user data: {ex.Message}");
            }
        }

        [HttpPost("migrate-user-data")]
        public async Task<IActionResult> MigrateUserData([FromBody] MigrateUserRequest request)
        {
            try
            {
                // Find both users
                var oldUser = await _userManager.Users.FirstOrDefaultAsync(u => u.UserName == request.FromUsername);
                var newUser = await _userManager.Users.FirstOrDefaultAsync(u => 
                    u.UserName == request.ToUsername || u.Email == request.ToUsername);

                if (oldUser == null)
                {
                    return NotFound($"Source user '{request.FromUsername}' not found");
                }

                if (newUser == null)
                {
                    return NotFound($"Target user '{request.ToUsername}' not found");
                }

                using var transaction = await _context.Database.BeginTransactionAsync();

                try
                {
                    // Get all CustomerReceipts for the old user
                    var customerReceipts = await _context.CustomerReceipts
                        .Where(cr => cr.UserId == oldUser.Id)
                        .ToListAsync();

                    // Update all CustomerReceipts to point to the new user
                    foreach (var customerReceipt in customerReceipts)
                    {
                        customerReceipt.UserId = newUser.Id;
                    }

                    await _context.SaveChangesAsync();
                    await transaction.CommitAsync();

                    var result = new
                    {
                        Success = true,
                        Message = $"Successfully migrated {customerReceipts.Count} receipt(s) from '{oldUser.UserName}' to '{newUser.UserName}'",
                        FromUser = new { oldUser.Id, oldUser.UserName, oldUser.Email },
                        ToUser = new { newUser.Id, newUser.UserName, newUser.Email },
                        MigratedReceiptsCount = customerReceipts.Count
                    };

                    return Ok(result);
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    return StatusCode(500, $"Migration failed and was rolled back: {ex.Message}");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error during migration: {ex.Message}");
            }
        }

        [HttpGet("list-all-users")]
        public async Task<IActionResult> ListAllUsers()
        {
            try
            {
                var users = await _userManager.Users
                    .Select(u => new { u.Id, u.UserName, u.Email, u.FullName })
                    .ToListAsync();

                return Ok(users);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error listing users: {ex.Message}");
            }
        }
    }

    public class MigrateUserRequest
    {
        public string FromUsername { get; set; } = string.Empty;
        public string ToUsername { get; set; } = string.Empty;
    }
}