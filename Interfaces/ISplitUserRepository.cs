using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using split_api.DTO.SplitUser;
using split_api.Helpers;
using split_api.Models;

namespace split_api.Interfaces
{
    public interface ISplitUserRepository
    {
        Task<List<SplitUser>> GetAllAsync(UserQueryObject query);
        Task<SplitUser?> GetByIdAsync(int id);
        Task<SplitUser> CreateAsync(SplitUser userModel);
        Task<SplitUser?> DeleteAsync(int id);
        Task<SplitUser?> UpdateAsync(int id, UpdateUserDto updateUserDto);
        Task<bool> userExist(int id);
    }
}