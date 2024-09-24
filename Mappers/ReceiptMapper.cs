using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using split_api.DTO.Receipt;
using split_api.Models;

namespace split_api.Mappers
{
    public static class ReceiptMapper
    {
        public static ReceiptDto ToReceiptDto(this Receipt receiptModel)
        {
            return new ReceiptDto
            {
                Id = receiptModel.Id,
                ReceiptCode = receiptModel.ReceiptCode,
                TransactionNumber = receiptModel.TransactionNumber,
                EstablishmentName = receiptModel.EstablishmentName,
                TransactionDateTime = receiptModel.TransactionDateTime,
            };
        }
    }
}