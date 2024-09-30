using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using split_api.Interfaces;
using split_api.Mappers;

namespace split_api.Controllers
{

    [Route("Item")]
    [ApiController]
    public class ItemController : ControllerBase
    {
        private readonly IItemRepository _itemRepo;

        public ItemController(IItemRepository itemRepo)
        {
            _itemRepo = itemRepo;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var items = await _itemRepo.GetAllItemAsync();
            var itemsDto = items.Select(i => i.ToItemtDto());

            return Ok(itemsDto);
        }

        [HttpGet("{id}")]

        public async Task<IActionResult> GetItemById([FromRoute] int id)
        {
            var item = await _itemRepo.GetItemByIdAsync(id);
            if (item is null) return NotFound();

            return Ok(item);
        }

        [HttpGet("ByReceiptId/{itemName}/{receiptId}")]
        public async Task<IActionResult> GetByReceiptId([FromRoute] int receiptId, string itemName)
        {
            var item = await _itemRepo.GetItemByReceiptIdAsync(receiptId, itemName);
            if (item is null) return NotFound();
            return Ok(item);
        }
    }
}