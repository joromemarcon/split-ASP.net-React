using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using split_api.Data;
using split_api.Helpers;
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

        public async Task<Item> CreateItemAsync(Item itemModel)
        {
            await _context.Items.AddAsync(itemModel);
            await _context.SaveChangesAsync();

            return itemModel;
        }

        public async Task<Item?> DeleteItemAsync(int id)
        {
            var itemModel = await _context.Items.FirstOrDefaultAsync(i => i.Id == id);
            if (itemModel is null) return null;

            _context.Items.Remove(itemModel);

            await _context.SaveChangesAsync();

            return itemModel;
        }

        public async Task<List<Item>> GetAllItemAsync(ItemQueryObject query)
        {
            var items = _context.Items.AsQueryable();
            if (!string.IsNullOrEmpty(query.ItemName) && query.ReceiptId != null)
            {
                items = items.Where(t => t.ItemName.Equals(query.ItemName)
                && t.ReceiptId.Equals(query.ReceiptId));
            }
            return await items.ToListAsync();
        }

        public async Task<Item?> GetItemByIdAsync(int id)
        {
            return await _context.Items.FindAsync(id);
        }

        // public async Task<Item?> GetItemByReceiptIdAsync(int receiptId, string itemName)
        // {
        //     return await _context.Items.FirstOrDefaultAsync(i => i.ReceiptId == receiptId && i.ItemName == itemName);
        // }

        public async Task<Item?> UpdateItemAsync(int id, Item itemModel)
        {
            var existingItem = await _context.Items.FindAsync(id);
            if (existingItem is null) return null;

            existingItem.ItemName = itemModel.ItemName;
            existingItem.ItemPrice = itemModel.ItemPrice;

            await _context.SaveChangesAsync();

            return existingItem;
        }
    }
}