using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
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
        public async Task<IActionResult> GetAll()
        {
            var receipt = await _receiptRepo.GetAllReceiptAsync();
            var receiptDto = receipt.Select(r => r.ToReceiptDto());

            return Ok(receiptDto);
        }

        /*
            GET REQUEST
        */

        [HttpGet("GetReceiptById/{id}")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            var receipt = await _receiptRepo.GetReceiptByIdAsync(id);
            if (receipt is null) return NotFound();

            return Ok(receipt);
        }
    }
}