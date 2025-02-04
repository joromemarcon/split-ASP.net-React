using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using split_api.Models;

namespace split_api.Data
{
    //Passing in SplitUser into IdentityDBContext so DBcontext doesnt default to IdentityUser class
    public class DataContext : IdentityDbContext<SplitUser>
    {
        public DataContext(DbContextOptions<DataContext> dbContextOptions) : base(dbContextOptions)
        {

        }

        public DbSet<SplitUser> SplitUsers { get; set; }
        public DbSet<Receipt> Receipts { get; set; }
        public DbSet<CustomerReceipt> CustomerReceipts { get; set; }
        public DbSet<Item> Items { get; set; }

        /*
            Defining Identity roles
            -   Every user created will be regular users for now

            ->  In the future if the receipt is received from
                application then user will be admin
                Otherwise, if receipt link is shared by admin
                then user will be a regular user.
                (See more in SplitUserController.cs, under Register POST request)

        */
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<CustomerReceipt>(x => x.HasKey(p => new { p.UserId, p.ReceiptId }));

            builder.Entity<CustomerReceipt>()
                .HasOne(u => u.SplitUser)
                .WithMany(u => u.CustomerReceipt)
                .HasForeignKey(p => p.UserId);

            builder.Entity<CustomerReceipt>()
                .HasOne(u => u.Receipt)
                .WithMany(u => u.CustomerReceipt)
                .HasForeignKey(p => p.ReceiptId);

            List<IdentityRole> roles = new List<IdentityRole>
            {
                new IdentityRole
                {
                    Name = "Admin",
                    NormalizedName = "ADMIN"
                },
                new IdentityRole
                {
                    Name = "User",
                    NormalizedName = "USER"
                }
            };
            builder.Entity<IdentityRole>().HasData(roles);
        }
    }
}