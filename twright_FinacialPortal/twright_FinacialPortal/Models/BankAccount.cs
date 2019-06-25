using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using twright_FinacialPortal.Enumerations;

namespace twright_FinacialPortal.Models
{
    public class BankAccount
    {
        public int Id { get; set; }

        public int HouseholdId { get; set; }

        public string Name { get; set; }

        public DateTime Opened { get; set; }

        public AccountType AccountType { get; set; }

        public decimal StartingBalance { get; set; }

        public decimal CurrentBalance { get; set; }

        public decimal LowBalanceLevel { get; set; }


        //nav 
        public virtual Household Household { get; set; }

        //Children
        public virtual ICollection<Transaction> Transactions { get; set; }

        public BankAccount()
        {
            Transactions = new HashSet<Transaction>();
        }
    }
}