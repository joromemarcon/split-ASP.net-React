using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace split_api.DTO.CustomerReceipt
{
    public class CustomerReceiptDto
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int ReceiptId { get; set; }
        public bool isOwner { get; set; }
        public bool IsPaid { get; set; }
        public DateTime DateTimePaid { get; set; }
    }
}