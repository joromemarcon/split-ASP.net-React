using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using split_api.Interfaces;

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
    }
}