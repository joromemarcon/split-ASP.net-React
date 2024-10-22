using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using split_api.Models;

namespace split_api.Interfaces
{
    public interface ICustomerReceiptRepository
    {
        Task<List<CustomerReceipt>> GetAllCRAsync();
        Task<CustomerReceipt?> GetCustomerReceiptByIdAsync(int id);
        Task<CustomerReceipt?> GetReceiptIdByReceiptIdAsync(int userId, int receiptId);
        Task<CustomerReceipt?> CreateCustomerReceiptAsync(CustomerReceipt customerReceipt);
        Task<CustomerReceipt?> UpdateCustomerReceiptAsync(int id, CustomerReceipt customerReceiptModel);
        Task<CustomerReceipt?> DeleteCustomerReceiptByIdAsync(int id);
    }
}