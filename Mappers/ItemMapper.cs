using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using split_api.DTO.Item;
using split_api.Models;

namespace split_api.Mappers
{
    public static class ItemMapper
    {
        public static ItemDto ToItemtDto(this Item itemModel)
        {
            return new ItemDto
            {
                Id = itemModel.Id,
                ItemName = itemModel.ItemName,
                ItemPrice = itemModel.ItemPrice
            };
        }
    }
}