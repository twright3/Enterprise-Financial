using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using twright_FinacialPortal.ExtensionMethods;
using twright_FinacialPortal.Models;

namespace twright_FinacialPortal.Controllers
{
    public class TransactionsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Transactions
        public ActionResult Index()
        {
            var transactions = db.Transactions.Include(t => t.BankAccount).Include(t => t.BudgetCategoryItem);
            return View(transactions.ToList());
        }

        // GET: Transactions/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Transaction transaction = db.Transactions.Find(id);
            if (transaction == null)
            {
                return HttpNotFound();
            }
            return View(transaction);
        }

        // GET: Transactions/Create
        public ActionResult Create()
        {
            ViewBag.BankAccountId = new SelectList(db.BankAccounts, "Id", "Name");
            ViewBag.BudgetCategoryItemId = new SelectList(db.BudgetCategoryItems, "Id", "Name");
            return View();
        }

        // POST: Transactions/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "BankAccountId,BudgetCategoryItemId,Amount,TransactionType,Payee,Memo,Reconciled,ReconciledDate,ReconciledAmount")] Transaction transaction)
        {
            if (ModelState.IsValid)
            {
                transaction.Entered = DateTime.Now;
                db.Transactions.Add(transaction);
                db.SaveChanges();

                transaction.UpdateAccountBalance();

                transaction.NotifyOnBalanceIssues();

                return RedirectToAction("Dashboard", "Houseolds");
            }

            ViewBag.BankAccountId = new SelectList(db.BankAccounts, "Id", "Name", transaction.BankAccountId);
            ViewBag.BudgetCategoryItemId = new SelectList(db.BudgetCategoryItems, "Id", "Name", transaction.BudgetCategoryItemId);
            return View(transaction);
        }

        // GET: Transactions/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Transaction transaction = db.Transactions.Find(id);
            if (transaction == null)
            {
                return HttpNotFound();
            }
            ViewBag.BankAccountId = new SelectList(db.BankAccounts, "Id", "Name", transaction.BankAccountId);
            ViewBag.BudgetCategoryItemId = new SelectList(db.BudgetCategoryItems, "Id", "Name", transaction.BudgetCategoryItemId);
            return View(transaction);
        }

        // POST: Transactions/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,BankAccountId,BudgetCategoryItemId,Amount,TransactionType,Payee,Memo,Entered,Reconciled,ReconciledDate,ReconciledAmount")] Transaction transaction)
        {
            if (ModelState.IsValid)
            {
                db.Entry(transaction).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.BankAccountId = new SelectList(db.BankAccounts, "Id", "Name", transaction.BankAccountId);
            ViewBag.BudgetCategoryItemId = new SelectList(db.BudgetCategoryItems, "Id", "Name", transaction.BudgetCategoryItemId);
            return View(transaction);
        }

        // GET: Transactions/Delete/5
        //public ActionResult Delete(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    Transaction transaction = db.Transactions.Find(id);
        //    if (transaction == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(transaction);
        //}

        // POST: Transactions/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public ActionResult DeleteConfirmed(int id)
        //{
        //    Transaction transaction = db.Transactions.Find(id);
        //    db.Transactions.Remove(transaction);
        //    db.SaveChanges();
        //    return RedirectToAction("Index");
        //}

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
