using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using twright_FinacialPortal.Enumerations;
using twright_FinacialPortal.Models;

namespace twright_FinacialPortal.ExtensionMethods
{
    public static class HouseholdExtenstions
    {
        private static ApplicationDbContext db = new ApplicationDbContext();
        public static bool HasAccounts(this Household house)
        {
            return house.BankAccounts.Count() > 0;
        }
        public static bool HasAccounts(this Household house, AccountType type)
        {
            return house.BankAccounts.Select(b => b.AccountType == type).Count() > 0;
        }
    }
}