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
    public class SplitUserRepository : ISplitUserRepository
    {
        private readonly DataContext _context;

        public SplitUserRepository(DataContext context)
        {
            _context = context;
        }

        public async Task<List<SplitUser>> GetAllAsync()
        {
            return await _context.SplitUsers.ToListAsync();
        }

        public async Task<SplitUser?> GetByIdAsync(int id)
        {
            return await _context.SplitUsers.FindAsync(id);
        }
    }
}