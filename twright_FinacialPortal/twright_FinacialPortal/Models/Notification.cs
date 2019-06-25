using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace twright_FinacialPortal.Models
{
    public class Notification
    {
        public int Id { get; set; }

        public int HouseholdId { get; set; }

        public DateTime Created { get; set; }

        public string Subject { get; set; }

        public string NotificationBody { get; set; }

        public bool Read { get; set; }


        //nav
        public virtual Household Household { get; set; }
    }
}