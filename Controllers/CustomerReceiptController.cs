using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using split_api.DTO.CustomerReceipt;
using split_api.Interfaces;
using split_api.Mappers;

namespace split_api.Controllers
{
    [Route("CustomerReceipt")]
    [ApiController]
    public class CustomerReceiptController : ControllerBase
    {
        private readonly ICustomerReceiptRepository _customerReceiptRepo;
        private readonly IReceiptRepository _receiptRepo;
        private readonly ISplitUserRepository _userRepository;

        public CustomerReceiptController(ICustomerReceiptRepository customerReceiptRepo, IReceiptRepository receiptRepo, ISplitUserRepository userRepository)
        {
            _customerReceiptRepo = customerReceiptRepo;
            _receiptRepo = receiptRepo;
            _userRepository = userRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllCR()
        {
            var customerReceipt = await _customerReceiptRepo.GetAllCRAsync();
            var customerReceiptDto = customerReceipt.Select(c => c.ToCustomerReceiptDto());

            return Ok(customerReceiptDto);

        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetCustomerReceiptById([FromRoute] int id)
        {
            var customerReceipt = await _customerReceiptRepo.GetCustomerReceiptByIdAsync(id);
            if (customerReceipt is null) return NotFound();

            return Ok(customerReceipt);
        }

        [HttpGet("{userId}/{receiptId}")]
        public async Task<IActionResult> GetReceiptIdByCustomerId([FromRoute] int userId, int receiptId)
        {
            var customerReceipt = await _customerReceiptRepo.GetReceiptIdByReceiptIdAsync(userId, receiptId);
            if (customerReceipt is null) return NotFound();

            return Ok(customerReceipt);
        }

        [HttpPost("{userId}/{receiptId}")]
        public async Task<IActionResult> CreateCustomerReceipt([FromRoute] int userId, int receiptId, CreateCustomerReceiptDto customerReceiptDto)
        {
            if (!await _userRepository.userExist(userId)) return NotFound("User Does Not Exist!");
            if (!await _receiptRepo.receiptExists(receiptId)) return NotFound("Receipt Not Found!");

            var customerReceipt = customerReceiptDto.ToCrFromCreateCr(userId, receiptId);
            await _customerReceiptRepo.CreateCustomerReceiptAsync(customerReceipt);

            return CreatedAtAction(nameof(GetCustomerReceiptById), new { id = customerReceipt.Id }, customerReceipt.ToCustomerReceiptDto());

        }
    }
}