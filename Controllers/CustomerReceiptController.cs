using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using split_api.Extensions;

// using split_api.DTO.CustomerReceipt;
using split_api.Helpers;
using split_api.Interfaces;
using split_api.Mappers;
using split_api.Models;

namespace split_api.Controllers
{
    [Route("CustomerReceipt")]
    [ApiController]
    public class CustomerReceiptController : ControllerBase
    {
        private readonly UserManager<SplitUser> _userManager;
        private readonly IReceiptRepository _receiptRepo;
        private readonly ICustomerReceiptRepository _customerReceiptRepo;
        public CustomerReceiptController(UserManager<SplitUser> userManager,
                                        IReceiptRepository receiptRepo,
                                        ICustomerReceiptRepository customerReceiptRepo)
        {
            _userManager = userManager;
            _receiptRepo = receiptRepo;
            _customerReceiptRepo = customerReceiptRepo;
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetCustomerReceipt()
        {
            var username = User.GetUserName();
            var splitUser = await _userManager.FindByNameAsync(username);
            var userReceipt = await _customerReceiptRepo.GetCustomerReceipt(splitUser);
            return Ok(userReceipt);
        }

        /*
            Use to join an existing receipt as non-owner
        */

        // [HttpPost]
        // [Authorize]
        // public async Task<IActionResult> JoinReceipt(string receiptCode, bool isOwner)
        // {
        //     var username = User.GetUserName();
        //     var splitUser = await _userManager.FindByNameAsync(username);
        //     var receipt = await _receiptRepo.GetReceiptByReceiptCode(receiptCode);

        //     if (receipt == null) return BadRequest("Receipt not found!");

        //     var customerReceipt = await _customerReceiptRepo.GetCustomerReceipt(splitUser);

        //     if (customerReceipt.Any(e => e.ReceiptCode.ToLower() == receiptCode.ToLower())) return BadRequest("Cannot Add Receipt!");

        //     var customerReceiptModel = new CustomerReceipt
        //     {
        //         UserId = splitUser.Id,
        //         ReceiptId = receipt.Id,
        //         IsPaid = false,
        //         isOwner = false,
        //         DateTimePaid = DateTime.MinValue

        //     };

        //     await _customerReceiptRepo.CreateAsync(customerReceiptModel);
        //     return Ok("Successfully joined receipt!");
        // }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> AddCustomerReceipt(string receiptCode, bool isOwner = false)
        {
            var username = User.GetUserName();
            var splitUser = await _userManager.FindByNameAsync(username);
            var receipt = await _receiptRepo.GetReceiptByReceiptCode(receiptCode);

            if (receipt == null) return BadRequest("Receipt not found!");

            var customerReceipt = await _customerReceiptRepo.GetCustomerReceipt(splitUser);

            if (customerReceipt.Any(e => e.ReceiptCode.ToLower() == receiptCode.ToLower())) return BadRequest("Cannot Add Receipt!");

            var customerReceiptModel = new CustomerReceipt
            {
                UserId = splitUser.Id,
                ReceiptId = receipt.Id,
                isOwner = isOwner,
                IsPaid = false,
                DateTimePaid = DateTime.MinValue
            };

            await _customerReceiptRepo.CreateAsync(customerReceiptModel);

            if (customerReceiptModel == null)
            {
                return StatusCode(500, "Could not created");
            }
            else
            {
                return Created();
            }
        }

        [HttpDelete]
        [Authorize]
        public async Task<IActionResult> RemoveCustomerReceipt(string receipt)
        {
            var username = User.GetUserName();
            var splitUser = await _userManager.FindByNameAsync(username);

            var userReceipt = await _customerReceiptRepo.GetCustomerReceipt(splitUser);

            var filteredReceipt = userReceipt.Where(r => r.ReceiptCode.ToLower() == receipt.ToLower()).ToList();

            if (filteredReceipt.Count() == 1)
            {
                await _customerReceiptRepo.DeleteCustomerReceipt(splitUser, receipt);
            }
            else
            {
                return BadRequest("Receipt does not exist with this user");
            }

            return Ok();


        }

        //         private readonly ICustomerReceiptRepository _customerReceiptRepo;
        //         private readonly IReceiptRepository _receiptRepo;
        //         private readonly ISplitUserRepository _userRepository;

        //         public CustomerReceiptController(ICustomerReceiptRepository customerReceiptRepo, IReceiptRepository receiptRepo, ISplitUserRepository userRepository)
        //         {
        //             _customerReceiptRepo = customerReceiptRepo;
        //             _receiptRepo = receiptRepo;
        //             _userRepository = userRepository;
        //         }

        //         [HttpGet]
        //         public async Task<IActionResult> GetAllCR([FromQuery] CustomerReceiptQueryObject query)
        //         {
        //             var customerReceipt = await _customerReceiptRepo.GetAllCRAsync(query);
        //             var customerReceiptDto = customerReceipt.Select(c => c.ToCustomerReceiptDto());

        //             return Ok(customerReceiptDto);

        //         }

        //         [HttpGet("{id}")]
        //         public async Task<IActionResult> GetCustomerReceiptById([FromRoute] int id)
        //         {
        //             var customerReceipt = await _customerReceiptRepo.GetCustomerReceiptByIdAsync(id);
        //             if (customerReceipt is null) return NotFound();

        //             return Ok(customerReceipt);
        //         }

        //         [HttpPost("{userId}/{receiptId}/{isOwner}")]
        //         public async Task<IActionResult> CreateCustomerReceipt([FromRoute] string userId, int receiptId, bool isOwner, CreateCustomerReceiptDto customerReceiptDto)
        //         {
        //             if (!await _userRepository.userExist(userId)) return NotFound("User Does Not Exist!");
        //             if (!await _receiptRepo.receiptExists(receiptId)) return NotFound("Receipt Not Found!");

        //             var customerReceipt = customerReceiptDto.ToCrFromCreateCr(userId, receiptId, isOwner);
        //             await _customerReceiptRepo.CreateCustomerReceiptAsync(customerReceipt);

        //             return CreatedAtAction(nameof(GetCustomerReceiptById), new { id = customerReceipt.Id }, customerReceipt.ToCustomerReceiptDto());

        //         }

        //         [HttpPut("{id}")]
        //         public async Task<IActionResult> UpdateCustomerReceipt([FromRoute] int id, [FromBody] UpdateCustomerReceiptDto updateDto)
        //         {
        //             var customerReceipt = await _customerReceiptRepo.UpdateCustomerReceiptAsync(id, updateDto.ToCrFromUpdateCr());
        //             if (customerReceipt is null) return NotFound("Customer Receipt not Found!");

        //             return Ok(customerReceipt.ToCustomerReceiptDto());
        //         }

        //         [HttpDelete("{id}")]
        //         public async Task<IActionResult> DeleteCustomerReceipt([FromRoute] int id)
        //         {
        //             var customerReceipt = await _customerReceiptRepo.DeleteCustomerReceiptByIdAsync(id);
        //             if (customerReceipt is null) return NotFound();

        //             return NoContent();
        //         }

    }
}