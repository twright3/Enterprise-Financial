using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace twright_FinacialPortal.Enumerations
{
    public enum TransactionType
    {
        Deposit,
        Withdrawal,
        [Display(Name = "Adjustment Up")]
        AdjustmentUp,
        [Display(Name = "Adjustment Down")]
        Adjustmentdown,
        Reconciliation
    }
}