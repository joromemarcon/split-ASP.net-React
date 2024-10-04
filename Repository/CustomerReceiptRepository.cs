using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;
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
        public async Task<List<CustomerReceipt>> GetAllCRAsync()
        {
            return await _context.CustomerReceipts.ToListAsync();
        }

        public async Task<CustomerReceipt?> GetCustomerReceiptByIdAsync(int id)
        {
            return await _context.CustomerReceipts.FindAsync(id);
        }
    }

}