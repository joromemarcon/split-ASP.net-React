using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

// using System.Xml.Linq;
// using Microsoft.AspNetCore.Http.HttpResults;
// using Microsoft.EntityFrameworkCore;
using split_api.Data;
// using split_api.Helpers;
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
        public async Task<List<Receipt>> GetCustomerReceipt(SplitUser user)
        {
            return await _context.CustomerReceipts.Where(x => x.UserId == user.Id)
            .Select(receipt => new Receipt
            {
                Id = receipt.Id,
                ReceiptCode = receipt.Receipt.ReceiptCode,
                TransactionNumber = receipt.Receipt.TransactionNumber,
                EstablishmentName = receipt.Receipt.EstablishmentName,
                TransactionDateTime = receipt.Receipt.TransactionDateTime,
                TransactionTotal = receipt.Receipt.TransactionTotal,
                TransactionTax = receipt.Receipt.TransactionTax,
                TransactionTip = receipt.Receipt.TransactionTip
            }).ToListAsync();
        }


        //         private readonly DataContext _context;
        //         public CustomerReceiptRepository(DataContext context)
        //         {
        //             _context = context;
        //         }

        //         public async Task<CustomerReceipt?> CreateCustomerReceiptAsync(CustomerReceipt customerReceipt)
        //         {
        //             await _context.CustomerReceipts.AddAsync(customerReceipt);
        //             await _context.SaveChangesAsync();

        //             return customerReceipt;
        //         }

        //         public async Task<CustomerReceipt?> DeleteCustomerReceiptByIdAsync(int id)
        //         {
        //             var customerReceiptModel = await _context.CustomerReceipts.FirstOrDefaultAsync(c => c.Id == id);
        //             if (customerReceiptModel is null) return null;

        //             _context.CustomerReceipts.Remove(customerReceiptModel);
        //             await _context.SaveChangesAsync();

        //             return customerReceiptModel;
        //         }

        //         public async Task<List<CustomerReceipt>> GetAllCRAsync(CustomerReceiptQueryObject query)
        //         {
        //             var customerReceipt = _context.CustomerReceipts.AsQueryable();
        //             if (query.ReceiptId != null && query.UserId != null)
        //             {
        //                 customerReceipt = customerReceipt.Where(r => r.ReceiptId.Equals(query.ReceiptId) ||
        //                                         r.UserId.Equals(query.UserId));
        //             }
        //             return await customerReceipt.ToListAsync();
        //         }

        //         public async Task<CustomerReceipt?> GetCustomerReceiptByIdAsync(int id)
        //         {
        //             return await _context.CustomerReceipts.FindAsync(id);
        //         }

        //         public async Task<CustomerReceipt?> UpdateCustomerReceiptAsync(int id, CustomerReceipt customerReceiptModel)
        //         {
        //             var existingCustomerReceipt = await _context.CustomerReceipts.FindAsync(id);
        //             if (existingCustomerReceipt is null) return null;

        //             existingCustomerReceipt.isOwner = customerReceiptModel.isOwner;
        //             existingCustomerReceipt.IsPaid = customerReceiptModel.IsPaid;

        //             await _context.SaveChangesAsync();

        //             return existingCustomerReceipt;
        //         }
    }

}