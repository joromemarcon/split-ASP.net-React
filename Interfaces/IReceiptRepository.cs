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
    }
}