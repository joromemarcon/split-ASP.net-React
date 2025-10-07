using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

// using System.Xml.Linq;
// using Microsoft.AspNetCore.Http.HttpResults;
// using Microsoft.EntityFrameworkCore;
using split_api.Data;
// using split_api.Helpers;
using split_api.Interfaces;
using split_api.Models;

namespace split_api.Repository
{
    public class CustomerReceiptRepository : ICustomerReceiptRepository
    {
        private readonly DataContext _context;
        public CustomerReceiptRepository(DataContext context)
        {
            _context = context;
        }

        public async Task<CustomerReceipt> CreateAsync(CustomerReceipt customerReceipt)
        {
            await _context.CustomerReceipts.AddAsync(customerReceipt);
            await _context.SaveChangesAsync();

            return customerReceipt;
        }

        public async Task<List<Receipt>> GetCustomerReceipt(SplitUser user)
        {
            // First get all receipts with their items
            var receipts = await _context.CustomerReceipts
                .Where(x => x.UserId == user.Id)
                .Include(cr => cr.Receipt)
                .ThenInclude(r => r.Items)
                .ToListAsync();

            // Then get all users involved in these receipts to build a lookup
            var receiptIds = receipts.Select(r => r.ReceiptId).ToList();

            // Get all claimed item IDs from these receipts
            var claimedUserHashes = receipts
                .SelectMany(r => r.Receipt.Items)
                .Where(item => item.PaidCustomerId != -1)
                .Select(item => item.PaidCustomerId)
                .Distinct()
                .ToList();

            Console.WriteLine($"[CustomerReceiptRepository] Looking for users with hashes: {string.Join(", ", claimedUserHashes)}");

            // Get ALL users from the database and filter by hash match
            var allUsers = await _context.Users.ToListAsync();
            var usersInReceipts = allUsers
                .Where(u => claimedUserHashes.Contains(GetStableHashCode(u.Id)))
                .ToList();

            Console.WriteLine($"[CustomerReceiptRepository] Found {usersInReceipts.Count} users who claimed items");
            foreach (var u in usersInReceipts)
            {
                if (u != null)
                {
                    Console.WriteLine($"  User: {u.FullName} ({u.UserName}), ID: {u.Id}, Hash: {GetStableHashCode(u.Id)}");
                }
            }

            // Create a dictionary mapping hash -> full name (fallback to username if empty)
            var userHashToName = usersInReceipts
                .Where(u => u != null)
                .ToDictionary(u => GetStableHashCode(u.Id), u => !string.IsNullOrWhiteSpace(u.FullName) ? u.FullName : u.UserName);

            // Debug logging
            Console.WriteLine($"[CustomerReceiptRepository] User hash lookup table:");
            foreach (var kvp in userHashToName)
            {
                Console.WriteLine($"  Hash: {kvp.Key} -> Username: {kvp.Value}");
            }

            // Build the result with usernames populated
            return receipts.Select(receipt => new Receipt
            {
                Id = receipt.Receipt.Id,
                ReceiptCode = receipt.Receipt.ReceiptCode,
                TransactionNumber = receipt.Receipt.TransactionNumber,
                EstablishmentName = receipt.Receipt.EstablishmentName,
                TransactionDateTime = receipt.Receipt.TransactionDateTime,
                TransactionTotal = receipt.Receipt.TransactionTotal,
                TransactionTax = receipt.Receipt.TransactionTax,
                TransactionTip = receipt.Receipt.TransactionTip,
                Items = receipt.Receipt.Items.Select(item =>
                {
                    var itemResult = new Item
                    {
                        Id = item.Id,
                        ItemName = item.ItemName,
                        ItemPrice = item.ItemPrice,
                        PaidCustomerId = item.PaidCustomerId,
                        ReceiptId = item.ReceiptId,
                        PaidCustomerName = item.PaidCustomerId != -1 && userHashToName.ContainsKey(item.PaidCustomerId)
                            ? userHashToName[item.PaidCustomerId]
                            : null
                    };

                    // Debug logging for claimed items
                    if (item.PaidCustomerId != -1)
                    {
                        Console.WriteLine($"  Item '{item.ItemName}' - PaidCustomerId: {item.PaidCustomerId}, Found name: {itemResult.PaidCustomerName ?? "NOT FOUND"}");
                    }

                    return itemResult;
                }).ToList(),
                isOwner = receipt.isOwner
            }).ToList();
        }

        // Stable hash function that produces consistent results (matches ItemController)
        private static int GetStableHashCode(string str)
        {
            unchecked
            {
                int hash1 = 5381;
                int hash2 = hash1;

                for (int i = 0; i < str.Length && str[i] != '\0'; i += 2)
                {
                    hash1 = ((hash1 << 5) + hash1) ^ str[i];
                    if (i == str.Length - 1 || str[i + 1] == '\0')
                        break;
                    hash2 = ((hash2 << 5) + hash2) ^ str[i + 1];
                }

                return Math.Abs(hash1 + (hash2 * 1566083941));
            }
        }

        public async Task<CustomerReceipt> DeleteCustomerReceipt(SplitUser splitUser, string receiptCode)
        {
            var customerReceiptModel = await _context.CustomerReceipts.FirstOrDefaultAsync(x => x.UserId == splitUser.Id && x.Receipt.ReceiptCode.ToLower() == receiptCode.ToLower());

            if (customerReceiptModel == null) return null;

            _context.CustomerReceipts.Remove(customerReceiptModel);
            await _context.SaveChangesAsync();

            return customerReceiptModel;
        }

        public async Task<bool> IsUserOwnerOfReceipt(string userId, int receiptId)
        {
            var customerReceipt = await _context.CustomerReceipts
            .FirstOrDefaultAsync(cr => cr.UserId == userId && cr.ReceiptId == receiptId);

            return customerReceipt?.isOwner ?? false;
        }

    }

}