using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace split_api.Models
{
    [Table("SplitUsers")]
    public class SplitUser : IdentityUser
    {
        public string FullName { get; set; } = string.Empty;

        /****************************************************************
            Navigation Property: many-to-many (Customer and Receipt)
        ****************************************************************/
        public List<CustomerReceipt> CustomerReceipt { get; set; } = new List<CustomerReceipt>();
    }
}