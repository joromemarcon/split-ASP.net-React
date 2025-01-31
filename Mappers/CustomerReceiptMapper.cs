// using System;
// using System.Collections.Generic;
// using System.Linq;
// using System.Threading.Tasks;
// using split_api.DTO.CustomerReceipt;
// using split_api.Models;

// namespace split_api.Mappers
// {
//     public static class CustomerReceiptMapper
//     {
//         public static CustomerReceiptDto ToCustomerReceiptDto(this CustomerReceipt customerReceiptModel)
//         {
//             return new CustomerReceiptDto
//             {
//                 Id = customerReceiptModel.Id,
//                 UserId = customerReceiptModel.UserId,
//                 ReceiptId = customerReceiptModel.ReceiptId,
//                 isOwner = customerReceiptModel.isOwner,
//                 IsPaid = customerReceiptModel.IsPaid,
//                 DateTimePaid = customerReceiptModel.DateTimePaid

//             };
//         }

//         public static CustomerReceipt ToCrFromCreateCr(this CreateCustomerReceiptDto customerReceiptModel, string userId, int receiptId, bool isOwner)
//         {
//             return new CustomerReceipt
//             {
//                 UserId = userId,
//                 ReceiptId = receiptId,
//                 isOwner = isOwner,
//                 IsPaid = customerReceiptModel.IsPaid
//             };
//         }

//         public static CustomerReceipt ToCrFromUpdateCr(this UpdateCustomerReceiptDto customerReceiptDto)
//         {
//             return new CustomerReceipt
//             {
//                 isOwner = customerReceiptDto.isOwner,
//                 IsPaid = customerReceiptDto.IsPaid
//             };
//         }
//     }
// }