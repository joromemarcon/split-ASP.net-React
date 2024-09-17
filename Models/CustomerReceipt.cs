using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace split_api.Models
{
    public class CustomerReceipt
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int ReceiptId { get; set; }
        public bool IsPaid { get; set; }
    }
}