using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using split_api.Data;
using split_api.DTO.SplitUser;
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

        public async Task<SplitUser> CreateAsync(SplitUser userModel)
        {
            await _context.SplitUsers.AddAsync(userModel);
            await _context.SaveChangesAsync();

            return userModel;
        }

        public async Task<List<SplitUser>> GetAllAsync()
        {
            return await _context.SplitUsers.ToListAsync();
        }

        public async Task<SplitUser?> GetByIdAsync(int id)
        {
            return await _context.SplitUsers.FindAsync(id);
        }

        public async Task<SplitUser?> GetByName(string name)
        {
            return await _context.SplitUsers.FirstOrDefaultAsync(u => u.FullName == name);
        }

        public async Task<SplitUser?> DeleteAsync(int id)
        {
            var splitUserModel = await _context.SplitUsers.FirstOrDefaultAsync(u => u.Id == id);
            if (splitUserModel is null) return null;

            _context.SplitUsers.Remove(splitUserModel);
            await _context.SaveChangesAsync();

            return splitUserModel;
        }

        public async Task<SplitUser?> UpdateAsync(int id, UpdateUserDto updateUser)
        {
            var existingSplitUser = await _context.SplitUsers.FirstOrDefaultAsync(u => u.Id == id);
            if (existingSplitUser is null) return null;

            existingSplitUser.FullName = updateUser.FullName;
            existingSplitUser.PhoneNumber = updateUser.PhoneNumber;

            await _context.SaveChangesAsync();

            return existingSplitUser;
        }

        public Task<bool> userExist(int id)
        {
            return _context.SplitUsers.AnyAsync(u => u.Id == id);
        }
    }
}