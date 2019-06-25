using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using twright_FinacialPortal.Enumerations;
using twright_FinacialPortal.Helpers;
using twright_FinacialPortal.Models;

namespace twright_FinacialPortal.ExtensionMethods
{
    public static class UserExtension

    {
        public static RoleHelper roleHelper = new RoleHelper();
        public static bool IsAdmin(this ApplicationUser user)
        {
            return HttpContext.Current.User.IsInRole(PortalRole.Admin.ToString());
        }

        public static string GetRole(this ApplicationUser user)
        {
            return roleHelper.ListUserRoles(user.Id).FirstOrDefault();
        }

        public static bool OccupiesRole(this ApplicationUser user, string role)
        {
            return roleHelper.IsUserInRole(user.Id, role);
        }

    }
}