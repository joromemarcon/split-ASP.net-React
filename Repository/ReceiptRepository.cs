using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using split_api.Data;
using split_api.Interfaces;

namespace split_api.Repository
{
    public class ReceiptRepository : IReceiptRepository
    {
        private readonly DataContext _context;

        public ReceiptRepository(DataContext context)
        {
            _context = context;
        }
    }
}