using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using twright_FinacialPortal.ExtensionMethods;
using twright_FinacialPortal.Models;
using twright_FinacialPortal.ViewModels;

namespace twright_FinacialPortal.Controllers
{
    public class DashboardController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: DashBoard
        //[Authorize(Roles = "HeadofHousehold, Member")]

        public JsonResult BuildBudgetData()
        {
            var houseId = User.Identity.GetHouseholdId();

            var myBudgets = db.BudgetCategories.Where(b => b.HouseholdId == houseId).ToList();

            var barDataList = new List<BudgetBarData>();

            foreach (var budget in myBudgets)
            {
                var barData = new BudgetBarData();
                barData.budget = budget.Name;
                barData.target = budget.TargetAmount;



                foreach (var item in budget.BudgetCategoryItems.ToList())
                {
                    //todo: tighten up this actual calc...only trans in current month/week
                    barData.actual += db.Transactions.Where(t => t.BudgetCategoryItemId == item.Id).ToList().Sum(i => i.Amount);
                }

                barDataList.Add(barData);
            }

            return Json(barDataList);
        }
    }
}