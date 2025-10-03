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

        public async Task<CustomerReceipt> CreateAsync(CustomerReceipt customerReceipt)
        {
            await _context.CustomerReceipts.AddAsync(customerReceipt);
            await _context.SaveChangesAsync();

            return customerReceipt;
        }

        public async Task<List<Receipt>> GetCustomerReceipt(SplitUser user)
        {
            return await _context.CustomerReceipts.Where(x => x.UserId == user.Id)
            .Select(receipt => new Receipt
            {
                Id = receipt.Receipt.Id,
                ReceiptCode = receipt.Receipt.ReceiptCode,
                TransactionNumber = receipt.Receipt.TransactionNumber,
                EstablishmentName = receipt.Receipt.EstablishmentName,
                TransactionDateTime = receipt.Receipt.TransactionDateTime,
                TransactionTotal = receipt.Receipt.TransactionTotal,
                TransactionTax = receipt.Receipt.TransactionTax,
                TransactionTip = receipt.Receipt.TransactionTip,
                Items = receipt.Receipt.Items
            }).ToListAsync();
        }

        public async Task<CustomerReceipt> DeleteCustomerReceipt(SplitUser splitUser, string receiptCode)
        {
            var customerReceiptModel = await _context.CustomerReceipts.FirstOrDefaultAsync(x => x.UserId == splitUser.Id && x.Receipt.ReceiptCode.ToLower() == receiptCode.ToLower());

            if (customerReceiptModel == null) return null;

            _context.CustomerReceipts.Remove(customerReceiptModel);
            await _context.SaveChangesAsync();

            return customerReceiptModel;
        }

    }

}