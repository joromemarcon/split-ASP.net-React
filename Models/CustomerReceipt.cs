using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace split_api.Models
{
    [Table("CustomerReceipts")]
    public class CustomerReceipt
    {
        public int Id { get; set; }

        /****************************************************************
        Foreign Keys and Navigation Properties
            - Join table elements for User and Receipt m-to-m relationship
        ****************************************************************/
        public string UserId { get; set; } = string.Empty; //Customer Foreign Key
        public SplitUser? SplitUser { get; set; }

        public int ReceiptId { get; set; } //Receipt Foreign Key
        public Receipt? Receipt { get; set; }


        /****************************************************************
        Additional Data for CustomerReceipt
        ****************************************************************/
        public bool isOwner { get; set; } //remove and put in receipt model instead
        public bool IsPaid { get; set; } //change to PaidStatus and add paid amount
        public DateTime DateTimePaid { get; set; }

    }
}