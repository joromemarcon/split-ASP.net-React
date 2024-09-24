using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using split_api.Models;

namespace split_api.Interfaces
{
    public interface IReceiptRepository
    {
        Task<List<Receipt>> GetAllReceiptAsync();
        Task<Receipt?> GetReceiptByIdAsync(int id);
        Task<Receipt?> GetReceiptByTransactionNumberAsync(string tNumber);
        Task<Receipt> CreateReceiptAsync(Receipt receiptModel);
        Task<Receipt?> DeleteReceiptAsync(int id);
    }
}