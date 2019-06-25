using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace twright_FinacialPortal.Models
{
    public class BudgetCategory
    {
        public int Id { get; set; }

        public int HouseholdId { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public decimal TargetAmount { get; set; }


        //nav 
        public virtual Household Household { get; set; }

        //Children
        public virtual ICollection<BudgetCategoryItem> BudgetCategoryItems { get; set; }

        public BudgetCategory()
        {
            BudgetCategoryItems = new HashSet<BudgetCategoryItem>();
        }
    }
}