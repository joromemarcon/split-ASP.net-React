using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace split_api.DTO.CustomerReceipt
{
    public class CreateCustomerReceiptDto
    {
        public int UserId { get; set; }
        public int ReceiptId { get; set; }
        public bool isOwner { get; set; } = false;
        public bool IsPaid { get; set; } = false;
    }
}