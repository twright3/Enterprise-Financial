using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace twright_FinacialPortal.Models
{
    public class Invitation
    {
        public int Id { get; set; }

        public int HouseholdId { get; set; }

        public DateTime Created { get; set; }

        public string InvitedBody { get; set; }
        
        public int DaysValid { get; set; }

        public bool IsValid { get; set; }

        public bool Expirable { get; set; }

        public Guid Code { get; set; }

        public string RecipientEmail { get; set; }

        //nav

        public virtual Household Household { get; set; }

    }
}