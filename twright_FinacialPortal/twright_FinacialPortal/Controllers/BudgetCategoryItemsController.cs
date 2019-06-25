using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using twright_FinacialPortal.Models;

namespace twright_FinacialPortal.Controllers
{
    public class BudgetCategoryItemsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: BudgetCategoryItems
        public ActionResult Index()
        {
            var budgetCategoryItems = db.BudgetCategoryItems.Include(b => b.BudgetCategory);
            return View(budgetCategoryItems.ToList());
        }

        // GET: BudgetCategoryItems/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BudgetCategoryItem budgetCategoryItem = db.BudgetCategoryItems.Find(id);
            if (budgetCategoryItem == null)
            {
                return HttpNotFound();
            }
            return View(budgetCategoryItem);
        }

        // GET: BudgetCategoryItems/Create
        public ActionResult Create(int id)
        {
            var myBudgets = db.BudgetCategories.Where(b => b.HouseholdId == id).ToList();
            ViewBag.BudgetCategoryId = new SelectList(myBudgets, "Id", "Name");
            return View();
        }

        // POST: BudgetCategoryItems/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,BudgetCategoryId,Name,Description")] BudgetCategoryItem budgetCategoryItem)
        {
            if (ModelState.IsValid)
            {
                db.BudgetCategoryItems.Add(budgetCategoryItem);
                db.SaveChanges();
                return RedirectToAction("Dashboard", "Households");
            }

            var budgets = db.BudgetCategories.Where(b => b.HouseholdId == budgetCategoryItem.BudgetCategory.HouseholdId).ToList();
            ViewBag.BudgetCategoryId = new SelectList(budgets, "Id", "Name", budgetCategoryItem.BudgetCategoryId);
            return View(budgetCategoryItem);
        }

        // GET: BudgetCategoryItems/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BudgetCategoryItem budgetCategoryItem = db.BudgetCategoryItems.Find(id);
            if (budgetCategoryItem == null)
            {
                return HttpNotFound();
            }
            ViewBag.BudgetCategoryId = new SelectList(db.BudgetCategories, "Id", "Name", budgetCategoryItem.BudgetCategoryId);
            return View(budgetCategoryItem);
        }

        // POST: BudgetCategoryItems/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,BudgetCategoryId,Name,Description")] BudgetCategoryItem budgetCategoryItem)
        {
            if (ModelState.IsValid)
            {
                db.Entry(budgetCategoryItem).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.BudgetCategoryId = new SelectList(db.BudgetCategories, "Id", "Name", budgetCategoryItem.BudgetCategoryId);
            return View(budgetCategoryItem);
        }

        // GET: BudgetCategoryItems/Delete/5
        //public ActionResult Delete(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    BudgetCategoryItem budgetCategoryItem = db.BudgetCategoryItems.Find(id);
        //    if (budgetCategoryItem == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(budgetCategoryItem);
        //}

        // POST: BudgetCategoryItems/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public ActionResult DeleteConfirmed(int id)
        //{
        //    BudgetCategoryItem budgetCategoryItem = db.BudgetCategoryItems.Find(id);
        //    db.BudgetCategoryItems.Remove(budgetCategoryItem);
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
