using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using split_api.DTO.Receipt;
using split_api.Helpers;
using split_api.Interfaces;
using split_api.Mappers;

namespace split_api.Controllers
{
    [Route("Receipt")]
    [ApiController]
    public class ReceiptController : ControllerBase
    {
        private readonly IReceiptRepository _receiptRepo;

        public ReceiptController(IReceiptRepository receiptRepo)
        {
            _receiptRepo = receiptRepo;
        }


        /*
            GET REQUEST
        */
        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] ReceiptQueryObject query)
        {
            var receipt = await _receiptRepo.GetAllReceiptAsync(query);
            var receiptDto = receipt.Select(r => r.ToReceiptDto());

            return Ok(receiptDto);
        }

        /*
            GET REQUEST BY ID
        */

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            var receipt = await _receiptRepo.GetReceiptByIdAsync(id);
            if (receipt is null) return NotFound();

            return Ok(receipt.ToReceiptDto());
        }


        /*
            POST REQUEST
        */
        [HttpPost]
        public async Task<IActionResult> CreateReceipt([FromBody] CreateReceiptDto receiptDto)
        {
            var receiptModel = receiptDto.ToReceiptFromCreateReceiptDto();
            await _receiptRepo.CreateReceiptAsync(receiptModel);

            return CreatedAtAction(nameof(GetById), new { id = receiptModel.Id }, receiptModel.ToReceiptDto());
        }

        /*
            UPDATE REQUEST
        */
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateReceipt([FromRoute] int id, [FromBody] UpdateReceiptDto updateReceiptDto)
        {
            var receiptModel = await _receiptRepo.UpdateReceiptAsync(id, updateReceiptDto);
            if (receiptModel is null) return NotFound();
            return NoContent();
        }


        /*
            DELETE REQUEST
        */
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteReceipt([FromRoute] int id)
        {
            var receiptModel = await _receiptRepo.DeleteReceiptAsync(id);
            if (receiptModel is null) return NotFound();
            return NoContent();
        }
    }
}