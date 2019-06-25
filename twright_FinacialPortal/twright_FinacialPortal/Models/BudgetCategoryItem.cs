using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace twright_FinacialPortal.Models
{
    public class BudgetCategoryItem
    {
        public int Id { get; set; }

        public int BudgetCategoryId { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        //nav
        public virtual BudgetCategory BudgetCategory { get; set; }
    }
}