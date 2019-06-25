using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Web;

namespace twright_FinacialPortal.ExtensionMethods
{
    public static class IdentityExtensions
    {
        public static int GetHouseholdId(this IIdentity identity)
        {
            var claim = ((ClaimsIdentity)identity).FindFirst("HouseholdId");
            var customClaim = claim.Value;
            return Convert.ToInt32(customClaim);


            //int? houseId = null;
            //var claimsIdentity = (ClaimsIdentity)user;
            //var HouseholdClaim = claimsIdentity.Claims.FirstOrDefault(c => c.Type == "HouseholdId");
            //if (HouseholdClaim != null)
            //    houseId = Convert.ToInt32(HouseholdClaim.Value);
            //return houseId;
        }
    }
}