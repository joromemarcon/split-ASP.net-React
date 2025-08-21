using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using split_api.Data;
using split_api.DTO.Receipt;
//using split_api.DTO.SplitUser;
using split_api.Helpers;
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

        public async Task<List<Receipt>> GetAllReceiptAsync(ReceiptQueryObject query)
        {
            var receipt = _context.Receipts.AsQueryable();

            if (!string.IsNullOrEmpty(query.ReceiptCode) || !string.IsNullOrEmpty(query.TransactionNumber))
            {
                receipt = receipt.Where(r => r.ReceiptCode.Equals(query.ReceiptCode) ||
                                        r.TransactionNumber.Equals(query.TransactionNumber));
            }

            return await receipt.Include(i => i.Items).ToListAsync();
        }

        public async Task<Receipt?> GetReceiptByIdAsync(int id)
        {
            return await _context.Receipts.Include(i => i.Items).FirstOrDefaultAsync(i => i.Id == id);
        }

        public async Task<Receipt?> GetReceiptByReceiptCode(string receiptCode)
        {
            return await _context.Receipts.FirstOrDefaultAsync(s => s.ReceiptCode == receiptCode);
        }

        public Task<bool> receiptExists(int id)
        {
            return _context.Receipts.AnyAsync(r => r.Id == id);
        }

        public async Task<Receipt?> UpdateReceiptAsync(int id, UpdateReceiptDto updateReceiptDto)
        {
            var existingReceipt = await _context.Receipts.FirstOrDefaultAsync(r => r.Id == id);
            if (existingReceipt is null) return null;

            existingReceipt.ReceiptCode = updateReceiptDto.ReceiptCode;
            existingReceipt.TransactionNumber = updateReceiptDto.TransactionNumber;
            existingReceipt.EstablishmentName = updateReceiptDto.EstablishmentName;
            existingReceipt.TransactionDateTime = updateReceiptDto.TransactionDateTime;
            existingReceipt.TransactionTotal = updateReceiptDto.TransactionTotal;
            existingReceipt.TransactionTax = updateReceiptDto.TransactionTax;
            existingReceipt.TransactionTip = updateReceiptDto.TransactionTip;

            await _context.SaveChangesAsync();

            return existingReceipt;
        }
    }
}