using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace split_api.Models
{
    public class SplitUser
    {
        public int Id { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;

        /****************************************************************
            Navigation Property: many-to-many (Customer and Receipt)
        ****************************************************************/
        public List<CustomerReceipt> CustomerReceipt { get; set; } = new List<CustomerReceipt>();
    }
}