using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using split_api.Interfaces;
using split_api.Mappers;

namespace split_api.Controllers
{
    [Route("CustomerReceipt")]
    [ApiController]
    public class CustomerReceiptController : ControllerBase
    {
        private readonly ICustomerReceiptRepository _customerReceiptRepo;

        public CustomerReceiptController(ICustomerReceiptRepository customerReceiptRepo)
        {
            _customerReceiptRepo = customerReceiptRepo;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllCR()
        {
            var customerReceipt = await _customerReceiptRepo.GetAllCRAsync();
            var customerReceiptDto = customerReceipt.Select(c => c.ToCustomerReceiptDto());

            return Ok(customerReceiptDto);

        }
    }
}