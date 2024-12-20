using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using split_api.Data;
using split_api.Interfaces;
using split_api.Models;

namespace split_api.Repository
{
    public class CustomerReceiptRepository : ICustomerReceiptRepository
    {
        private readonly DataContext _context;
        public CustomerReceiptRepository(DataContext context)
        {
            _context = context;
        }

        public async Task<CustomerReceipt?> CreateCustomerReceiptAsync(CustomerReceipt customerReceipt)
        {
            await _context.CustomerReceipts.AddAsync(customerReceipt);
            await _context.SaveChangesAsync();

            return customerReceipt;
        }

        public async Task<CustomerReceipt?> DeleteCustomerReceiptByIdAsync(int id)
        {
            var customerReceiptModel = await _context.CustomerReceipts.FirstOrDefaultAsync(c => c.Id == id);
            if (customerReceiptModel is null) return null;

            _context.CustomerReceipts.Remove(customerReceiptModel);
            await _context.SaveChangesAsync();

            return customerReceiptModel;
        }

        public async Task<List<CustomerReceipt>> GetAllCRAsync()
        {
            return await _context.CustomerReceipts.ToListAsync();
        }

        public async Task<CustomerReceipt?> GetCustomerReceiptByIdAsync(int id)
        {
            return await _context.CustomerReceipts.FindAsync(id);
        }

        public async Task<CustomerReceipt?> GetReceiptIdByReceiptIdAsync(int userId, int receiptId)
        {
            return await _context.CustomerReceipts.FirstOrDefaultAsync(c => c.UserId == userId && c.ReceiptId == receiptId);
        }

        public async Task<CustomerReceipt?> UpdateCustomerReceiptAsync(int id, CustomerReceipt customerReceiptModel)
        {
            var existingCustomerReceipt = await _context.CustomerReceipts.FindAsync(id);
            if (existingCustomerReceipt is null) return null;

            existingCustomerReceipt.isOwner = customerReceiptModel.isOwner;
            existingCustomerReceipt.IsPaid = customerReceiptModel.IsPaid;

            await _context.SaveChangesAsync();

            return existingCustomerReceipt;
        }
    }

}