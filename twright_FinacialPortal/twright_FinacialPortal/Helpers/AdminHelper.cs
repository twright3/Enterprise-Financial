using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using twright_FinacialPortal.Models;

namespace twright_FinacialPortal.Helpers
{
    public class AdminHelper
    {
        private static ApplicationDbContext db = new ApplicationDbContext();
        public static async Task ReauthorizeUserAsync(string UserId)
        {
            IAuthenticationManager authenticationManager = HttpContext.Current.GetOwinContext().Authentication;
            authenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);

            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(db));

            var user = userManager.FindById(UserId);
            var identity = await userManager.CreateIdentityAsync(user, DefaultAuthenticationTypes.ApplicationCookie);

            authenticationManager.SignIn(new AuthenticationProperties() { IsPersistent = true }, identity);
        }

    }
}