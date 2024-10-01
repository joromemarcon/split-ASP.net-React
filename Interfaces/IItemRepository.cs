using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using split_api.Models;

namespace split_api.Interfaces
{
    public interface IItemRepository
    {
        Task<List<Item>> GetAllItemAsync();
        Task<Item?> GetItemByIdAsync(int id);
        Task<Item?> GetItemByReceiptIdAsync(int receiptId, string itemName);
        Task<Item> CreateItemAsync(Item itemModel);
    }
}