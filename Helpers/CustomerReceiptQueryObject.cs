using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace split_api.Helpers
{
    public class CustomerReceiptQueryObject
    {
        public int? ReceiptId { get; set; } = null;
        public int? UserId { get; set; } = null;
    }
}