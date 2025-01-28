using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace split_api.Helpers
{
    public class ReceiptQueryObject
    {
        public string? ReceiptCode { get; set; } = null;
        public string? TransactionNumber { get; set; } = null;
    }
}