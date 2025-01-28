using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace split_api.Helpers
{
    public class ItemQueryObject
    {
        public int? ReceiptId { get; set; } = null;
        public string? ItemName { get; set; } = null;
    }
}