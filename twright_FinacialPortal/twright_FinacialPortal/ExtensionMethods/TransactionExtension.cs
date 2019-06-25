using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using twright_FinacialPortal.Models;

namespace twright_FinacialPortal.ExtensionMethods
{
    public static class TransactionExtension
    {
        private static ApplicationDbContext db = new ApplicationDbContext();

        public static void UpdateAccountBalance(this Transaction transaction)
        {
            //Get Bank Account
            var account = db.BankAccounts.Find(transaction.BankAccountId);
            if (transaction.TransactionType.ToString() == "Withdrawal" || transaction.TransactionType.ToString() == "Adjustment down")
                account.CurrentBalance -= transaction.Amount;
            else
                account.CurrentBalance += transaction.Amount;

            db.SaveChanges();
        }

        public static void NotifyOnBalanceIssues(this Transaction transaction)
        {
            var bankAccount = db.BankAccounts.Find(transaction.BankAccountId);
            if (bankAccount.CurrentBalance < 0)
                transaction.SendOverDraftNotification(bankAccount);
            else if (bankAccount.CurrentBalance < bankAccount.LowBalanceLevel)
                transaction.SendLowBalanceNotification(bankAccount);
        }

        public static void SendOverDraftNotification(this Transaction transaction, BankAccount account)
        {
            var notification = new Notification
            {
                Created = DateTime.Now,
                HouseholdId = account.HouseholdId,
                Subject = "You have overdrafted your Account",
                NotificationBody = $"Your {account.Name} account has been overdrafted. Your most recent Transaction in the amount of ${transaction.Amount} has resulted in a balance of ${account.CurrentBalance}.",
                Read = false
            };
            db.Notifications.Add(notification);
            db.SaveChanges();
        }

        public static void SendLowBalanceNotification(this Transaction transaction, BankAccount account)
        {
            var notification = new Notification
            {
                Created = DateTime.Now,
                HouseholdId = account.HouseholdId,
                Subject = "You have reached the Low Level setting in your Account",
                NotificationBody = $"Your {account.Name} account has reached it's low-level balance and now sits at ${account.CurrentBalance}.",
                Read = false
            };
            db.Notifications.Add(notification);
            db.SaveChanges();
        }
    }
}