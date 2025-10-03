using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using split_api.DTO.Item;
using split_api.Extensions;
using split_api.Helpers;
using split_api.Interfaces;
using split_api.Mappers;
using split_api.Models;

namespace split_api.Controllers
{

    [Route("Item")]
    [ApiController]
    public class ItemController : ControllerBase
    {
        private readonly IItemRepository _itemRepo;
        private readonly IReceiptRepository _receiptRepo;
        private readonly ICustomerReceiptRepository _customerReceiptRepo;
        private readonly UserManager<SplitUser> _userManager;

        public ItemController(IItemRepository itemRepo, IReceiptRepository receiptRepo, ICustomerReceiptRepository customerReceiptRepo, UserManager<SplitUser> userManager)
        {
            _itemRepo = itemRepo;
            _receiptRepo = receiptRepo;
            _customerReceiptRepo = customerReceiptRepo;
            _userManager = userManager;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] ItemQueryObject query)
        {
            var items = await _itemRepo.GetAllItemAsync(query);
            var itemsDto = items.Select(i => i.ToItemDto());

            return Ok(itemsDto);
        }

        [HttpGet("{id}")]

        public async Task<IActionResult> GetItemById([FromRoute] int id)
        {
            var item = await _itemRepo.GetItemByIdAsync(id);
            if (item is null) return NotFound();

            return Ok(item);
        }

        // [HttpGet("ByReceiptId/{itemName}/{receiptId}")]
        // public async Task<IActionResult> GetByReceiptId([FromRoute] int receiptId, string itemName)
        // {
        //     var item = await _itemRepo.GetItemyReceiptIdAsync(receiptId, itemName);
        //     if (item is null) return NotFound();
        //     return Ok(item);
        // }

        [HttpPost("{receiptId}")]
        public async Task<IActionResult> CreateItem([FromRoute] int receiptId, CreateItemDto itemDto)
        {
            if (!await _receiptRepo.receiptExists(receiptId))
            {
                return BadRequest("Receipt Does Not Exist!");
            }

            var itemModel = itemDto.ToItemFromCreateItem(receiptId);
            await _itemRepo.CreateItemAsync(itemModel);

            return CreatedAtAction(nameof(GetItemById), new { id = itemModel.Id }, itemModel.ToItemDto());
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateItem([FromRoute] int id, [FromBody] UpdateItemRequestDto updateDto)
        {
            var item = await _itemRepo.UpdateItemAsync(id, updateDto.ToItemFromUpdateItem());
            if (item is null) return NotFound("Item Not Found!");

            return Ok(item.ToItemDto());
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteItem([FromRoute] int id)
        {
            var itemModel = await _itemRepo.DeleteItemAsync(id);
            if (itemModel is null) return NotFound();

            return NoContent();
        }

        [HttpPut("claim")]
        [Authorize]
        public async Task<IActionResult> ClaimItems([FromBody] List<int> itemIds)
        {
            if (itemIds == null || itemIds.Count == 0)
            {
                return BadRequest("No items provided");
            }

            // Get current user
            var username = User.GetUserName();
            var splitUser = await _userManager.FindByNameAsync(username);
            if (splitUser == null) return Unauthorized();

            // Get all items to validate
            var items = new List<Item>();
            foreach (var itemId in itemIds)
            {
                var item = await _itemRepo.GetItemByIdAsync(itemId);
                if (item == null) return NotFound($"Item with ID {itemId} not found");
                items.Add(item);
            }

            // Validate user is part of the receipt (owner or member)
            var receiptId = items.First().ReceiptId;
            if (items.Any(i => i.ReceiptId != receiptId))
            {
                return BadRequest("All items must belong to the same receipt");
            }

            var receipt = await _receiptRepo.GetReceiptByIdAsync(receiptId);
            if (receipt == null) return NotFound("Receipt not found");

            var userReceipts = await _customerReceiptRepo.GetCustomerReceipt(splitUser);
            if (!userReceipts.Any(r => r.Id == receiptId))
            {
                return Unauthorized("You are not part of this receipt");
            }

            // Claim all unpaid items
            var claimedCount = 0;
            // Use stable hash of user ID string (not GetHashCode which is unstable)
            var userIdHash = GetStableHashCode(splitUser.Id);

            Console.WriteLine($"[ItemController.ClaimItems] User '{splitUser.FullName}' ({splitUser.UserName}, ID: {splitUser.Id}) claiming items");
            Console.WriteLine($"[ItemController.ClaimItems] Calculated stable hash: {userIdHash}");

            foreach (var item in items)
            {
                if (item.PaidCustomerId == -1)
                {
                    item.PaidCustomerId = userIdHash;
                    await _itemRepo.UpdateItemAsync(item.Id, item);
                    Console.WriteLine($"[ItemController.ClaimItems] Claimed item '{item.ItemName}' with hash {userIdHash}");
                    claimedCount++;
                }
            }

            return Ok(new { message = $"Successfully claimed {claimedCount} items", claimedCount });
        }

        [HttpPut("unclaim")]
        [Authorize]
        public async Task<IActionResult> UnclaimItems([FromBody] List<int> itemIds)
        {
            if (itemIds == null || itemIds.Count == 0)
            {
                return BadRequest("No items provided");
            }

            // Get current user
            var username = User.GetUserName();
            var splitUser = await _userManager.FindByNameAsync(username);
            if (splitUser == null) return Unauthorized();

            var userIdHash = GetStableHashCode(splitUser.Id);

            // Get all items to validate
            var items = new List<Item>();
            foreach (var itemId in itemIds)
            {
                var item = await _itemRepo.GetItemByIdAsync(itemId);
                if (item == null) return NotFound($"Item with ID {itemId} not found");
                items.Add(item);
            }

            // Validate user is part of the receipt (owner or member)
            var receiptId = items.First().ReceiptId;
            if (items.Any(i => i.ReceiptId != receiptId))
            {
                return BadRequest("All items must belong to the same receipt");
            }

            var receipt = await _receiptRepo.GetReceiptByIdAsync(receiptId);
            if (receipt == null) return NotFound("Receipt not found");

            var userReceipts = await _customerReceiptRepo.GetCustomerReceipt(splitUser);
            var isOwner = userReceipts.FirstOrDefault(r => r.Id == receiptId)?.isOwner ?? false;

            // Unclaim items
            var unclaimedCount = 0;

            foreach (var item in items)
            {
                // Owner can unclaim any item, members can only unclaim their own items
                if (isOwner || item.PaidCustomerId == userIdHash)
                {
                    item.PaidCustomerId = -1;
                    await _itemRepo.UpdateItemAsync(item.Id, item);
                    unclaimedCount++;
                }
            }

            return Ok(new { message = $"Successfully unclaimed {unclaimedCount} items", unclaimedCount });
        }

        // Stable hash function that produces consistent results
        private static int GetStableHashCode(string str)
        {
            unchecked
            {
                int hash1 = 5381;
                int hash2 = hash1;

                for (int i = 0; i < str.Length && str[i] != '\0'; i += 2)
                {
                    hash1 = ((hash1 << 5) + hash1) ^ str[i];
                    if (i == str.Length - 1 || str[i + 1] == '\0')
                        break;
                    hash2 = ((hash2 << 5) + hash2) ^ str[i + 1];
                }

                return Math.Abs(hash1 + (hash2 * 1566083941));
            }
        }
    }
}