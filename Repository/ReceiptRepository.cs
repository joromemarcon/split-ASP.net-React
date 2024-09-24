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
    public class ReceiptRepository : IReceiptRepository
    {
        private readonly DataContext _context;

        public ReceiptRepository(DataContext context)
        {
            _context = context;
        }

        public async Task<Receipt> CreateReceiptAsync(Receipt receiptModel)
        {
            await _context.Receipts.AddAsync(receiptModel);
            await _context.SaveChangesAsync();

            return receiptModel;
        }

        public async Task<Receipt?> DeleteReceiptAsync(int id)
        {
            var receiptModel = await _context.Receipts.FirstOrDefaultAsync(r => r.Id == id);
            if (receiptModel is null) return null;

            _context.Receipts.Remove(receiptModel);
            await _context.SaveChangesAsync();

            return receiptModel;
        }

        public async Task<List<Receipt>> GetAllReceiptAsync()
        {
            return await _context.Receipts.ToListAsync();
        }

        public async Task<Receipt?> GetReceiptByIdAsync(int id)
        {
            return await _context.Receipts.FindAsync(id);
        }

        public async Task<Receipt?> GetReceiptByTransactionNumberAsync(string tNumber)
        {
            return await _context.Receipts.FirstOrDefaultAsync(r => r.TransactionNumber == tNumber);
        }
    }
}