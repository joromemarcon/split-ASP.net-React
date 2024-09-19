using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using split_api.Models;

namespace split_api.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> dbContextOptions) : base(dbContextOptions)
        {

        }

        public DbSet<SplitUser> SplitUsers { get; set; }
        public DbSet<Receipt> Receipts { get; set; }
        public DbSet<CustomerReceipt> CustomerReceipts { get; set; }
        public DbSet<Item> Items { get; set; }
    }
}