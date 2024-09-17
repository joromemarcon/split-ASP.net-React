using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace split_api.Models
{
    public class Item
    {
        public int Id { get; set; }
        public int ReceiptId { get; set; }
        public string ItemName { get; set; } = string.Empty;
        public decimal ItemPrice { get; set; }
        public int PaidCustomerId { get; set; }
    }
}