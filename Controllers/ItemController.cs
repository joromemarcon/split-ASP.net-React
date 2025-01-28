using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using split_api.DTO.Item;
using split_api.Helpers;
using split_api.Interfaces;
using split_api.Mappers;

namespace split_api.Controllers
{

    [Route("Item")]
    [ApiController]
    public class ItemController : ControllerBase
    {
        private readonly IItemRepository _itemRepo;
        private readonly IReceiptRepository _receiptRepo;

        public ItemController(IItemRepository itemRepo, IReceiptRepository receiptRepo)
        {
            _itemRepo = itemRepo;
            _receiptRepo = receiptRepo;
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
    }
}