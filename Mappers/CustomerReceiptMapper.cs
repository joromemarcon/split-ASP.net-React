using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using split_api.DTO.CustomerReceipt;
using split_api.Models;

namespace split_api.Mappers
{
    public static class CustomerReceiptMapper
    {
        public static CustomerReceiptDto ToCustomerReceiptDto(this CustomerReceipt customerReceiptModel)
        {
            return new CustomerReceiptDto
            {
                Id = customerReceiptModel.Id,
                UserId = customerReceiptModel.UserId,
                ReceiptId = customerReceiptModel.ReceiptId,
                isOwner = customerReceiptModel.isOwner,
                IsPaid = customerReceiptModel.IsPaid,
                DateTimePaid = customerReceiptModel.DateTimePaid

            };
        }

        public static CustomerReceipt ToCrFromCreateCr(this CreateCustomerReceiptDto customerReceiptModel, int userId, int receiptId)
        {
            return new CustomerReceipt
            {
                UserId = userId,
                ReceiptId = receiptId,
                isOwner = customerReceiptModel.isOwner,
                IsPaid = customerReceiptModel.IsPaid
            };
        }
    }
}