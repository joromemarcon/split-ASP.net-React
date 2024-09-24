using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace split_api.DTO.Receipt
{
    public class ReceiptDto
    {
        public int Id { get; set; }
        public string ReceiptCode { get; set; } = string.Empty;
        public string TransactionNumber { get; set; } = string.Empty;
        public string EstablishmentName { get; set; } = string.Empty;
        public string TransactionDateTime { get; set; } = string.Empty;
    }
}