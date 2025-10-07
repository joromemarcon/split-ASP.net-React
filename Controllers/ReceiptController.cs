using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using split_api.DTO.Receipt;
using split_api.Helpers;
using split_api.Interfaces;
using split_api.Mappers;
using split_api.Extensions;
using split_api.Models;

namespace split_api.Controllers
{
    [Route("Receipt")]
    [ApiController]
    public class ReceiptController : ControllerBase
    {
        private readonly IReceiptRepository _receiptRepo;
        private readonly ICustomerReceiptRepository _customerReceiptRepo;

        private readonly UserManager<SplitUser> _userManager;
        public ReceiptController(IReceiptRepository receiptRepo, ICustomerReceiptRepository customerReceiptRepo, UserManager<SplitUser> userManager)
        {
            _receiptRepo = receiptRepo;
            _customerReceiptRepo = customerReceiptRepo;
            _userManager = userManager;

        }


        /*
            GET REQUEST
        */
        [HttpGet]
        [Authorize]
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
        [Authorize]
        public async Task<IActionResult> UpdateReceipt([FromRoute] int id, [FromBody] UpdateReceiptDto updateReceiptDto)
        {
            /*
                This method currently allows current user to edit owned receipts
            */

            //get current user from JWT token
            var username = User.GetUserName();
            var splitUser = await _userManager.FindByNameAsync(username);

            if (splitUser == null) return Unauthorized();

            //Check if user is owner of receipt
            var isOwner = await _customerReceiptRepo.IsUserOwnerOfReceipt(splitUser.Id, id);

            if (!isOwner) return StatusCode(403, "Only the receipt hose can edit this receipt");

            //proceed with updating receipt
            var receiptModel = await _receiptRepo.UpdateReceiptAsync(id, updateReceiptDto);
            if (receiptModel is null) return NotFound();

            return NoContent();

            // var receiptModel = await _receiptRepo.UpdateReceiptAsync(id, updateReceiptDto);
            // if (receiptModel is null) return NotFound();
            // return NoContent();
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