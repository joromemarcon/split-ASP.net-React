using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using split_api.DTO.SplitUser;
using split_api.Models;

namespace split_api.Mappers
{
    public static class SplitUserMapper
    {
        public static SplitUserDto ToSplitUserDto(this SplitUser splitUserModel)
        {
            return new SplitUserDto
            {
                Id = splitUserModel.Id,
                FullName = splitUserModel.FullName,
            };
        }
    }
}