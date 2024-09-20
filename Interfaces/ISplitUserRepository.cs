using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using split_api.Models;

namespace split_api.Interfaces
{
    public interface ISplitUserRepository
    {
        Task<List<SplitUser>> GetAllAsync();
        Task<SplitUser?> GetByIdAsync(int id);
    }
}