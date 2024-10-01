using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace split_api.DTO.Item
{
    public class UpdateItemRequestDto
    {
        public string ItemName { get; set; } = string.Empty;
        public decimal ItemPrice { get; set; }
    }
}