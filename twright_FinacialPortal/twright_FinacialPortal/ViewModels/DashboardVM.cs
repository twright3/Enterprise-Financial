using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using twright_FinacialPortal.Models;

namespace twright_FinacialPortal.ViewModels
{
    public class DashboardVM
    {
    }

    public class ConfigurationVM
    {
        public int HouseholdId { get; set; }

        public BankAccount Account { get; set; }

        public BudgetCategory Budget { get; set; }

        public BudgetCategoryItem Item { get; set; }

        public ConfigurationVM()
        {
            Account = new BankAccount();
            Budget = new BudgetCategory();
            Item = new BudgetCategoryItem();
        }

    }
}