using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace split_api.Models
{
    [Table("Receipts")]
    public class Receipt
    {
        public int Id { get; set; }
        public string ReceiptCode { get; set; } = string.Empty;
        public string TransactionNumber { get; set; } = string.Empty;
        public string EstablishmentName { get; set; } = string.Empty;
        public string TransactionDateTime { get; set; } = string.Empty;
        public decimal TransactionTotal { get; set; }
        public decimal TransactionTax { get; set; }
        public decimal TransactionTip { get; set; }

        /****************************************************************
            Additional Properties (Not Mapped to Database):

            isOwner - Indicates if the current user is the owner of this receipt
        ****************************************************************/
        [NotMapped]
        public bool? isOwner { get; set; }

        /****************************************************************
            Navigation Properties:

            List<Item> Items - One-to-Many (Receipt and Items)
            List<CustomerReceipt> CustomerReceipt - many-to-many (Customer and Receipt)
        ****************************************************************/
        public List<Item> Items { get; set; } = new List<Item>();
        public List<CustomerReceipt> CustomerReceipt { get; set; } = new List<CustomerReceipt>();
    }
}