using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using twright_FinacialPortal.Enumerations;

namespace twright_FinacialPortal.Models
{
    public class Transaction
    {
        public int Id { get; set; }

        public int BankAccountId { get; set; }

        public int? BudgetCategoryItemId { get; set; }

        public decimal Amount { get; set; }

        public TransactionType TransactionType { get; set; }

        public string Payee { get; set; }

        public string Memo { get; set; }

        public DateTime Entered { get; set; }

        public bool Reconciled { get; set; }

        public DateTime? ReconciledDate { get; set; }

        public decimal ReconciledAmount { get; set; }


        //Nav
        public virtual BankAccount BankAccount { get; set; }

        public virtual BudgetCategoryItem BudgetCategoryItem { get; set; }

    }
}