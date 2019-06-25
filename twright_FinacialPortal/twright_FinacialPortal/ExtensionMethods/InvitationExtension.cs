using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Mail;
using System.Security.Policy;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using twright_FinacialPortal.Models;

namespace twright_FinacialPortal.ExtensionMethods
{
    public static class InvitationExtension
    {
        public static ApplicationDbContext db = new ApplicationDbContext();
        public static async Task SendAsync(this Invitation invitation)
        {
            //Code up the sending of a new email to the recipient too picture of jason code
            try
            {
                var urlHelper = new UrlHelper(HttpContext.Current.Request.RequestContext);
                var callBackUrl = urlHelper.Action("InviteRegister", "Account", new {code = invitation.Code, email = invitation.RecipientEmail}, protocol: HttpContext.Current.Request.Url.Scheme);

                var body = $"{invitation.InvitedBody} <br /> Please click on link to begin the process of accepting your invitation. <a href=\"" + callBackUrl + "\">here</a>";
                var housename = db.Households.Find(invitation.HouseholdId).Name;
                var from = "twright_FinancialPortal <travwright3@gmail.com>";
                var email = new MailMessage(from, invitation.RecipientEmail)
                {
                    Subject = $"You have been invited to join {housename}.",
                    Body = $"<p>{body}</p>",
                    IsBodyHtml = true
                };

                var svc = new PersonalEmail();
                await svc.SendAsync(email);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                await Task.FromResult(0);
            }

        }
        
    }
}