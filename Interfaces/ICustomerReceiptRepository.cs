using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
// using split_api.Helpers;
using split_api.Models;

namespace split_api.Interfaces
{
    public interface ICustomerReceiptRepository
    {
        Task<List<Receipt>> GetCustomerReceipt(SplitUser user);
        Task<CustomerReceipt> CreateAsync(CustomerReceipt customerReceipt);
        //         Task<List<CustomerReceipt>> GetAllCRAsync(CustomerReceiptQueryObject query);
        //         Task<CustomerReceipt?> GetCustomerReceiptByIdAsync(int id);
        //         Task<CustomerReceipt?> CreateCustomerReceiptAsync(CustomerReceipt customerReceipt);
        //         Task<CustomerReceipt?> UpdateCustomerReceiptAsync(int id, CustomerReceipt customerReceiptModel);
        //         Task<CustomerReceipt?> DeleteCustomerReceiptByIdAsync(int id);
    }
}