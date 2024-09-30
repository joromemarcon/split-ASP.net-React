using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using split_api.Data;
using split_api.Interfaces;
using split_api.Models;

namespace split_api.Repository
{
    public class ItemRepository : IItemRepository
    {
        private readonly DataContext _context;
        public ItemRepository(DataContext context)
        {
            _context = context;
        }

        public async Task<List<Item>> GetAllItemAsync()
        {
            return await _context.Items.ToListAsync();
        }

        public async Task<Item?> GetItemByIdAsync(int id)
        {
            return await _context.Items.FindAsync(id);
        }

        public async Task<Item?> GetItemByReceiptIdAsync(int receiptId, string itemName)
        {
            return await _context.Items.FirstOrDefaultAsync(i => i.ReceiptId == receiptId && i.ItemName == itemName);
        }
    }
}