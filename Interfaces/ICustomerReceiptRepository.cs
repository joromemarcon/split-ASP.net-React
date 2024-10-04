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
    }
}