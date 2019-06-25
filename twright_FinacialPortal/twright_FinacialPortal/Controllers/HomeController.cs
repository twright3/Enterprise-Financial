using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using twright_FinacialPortal.Models;

namespace twright_FinacialPortal.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            Email model = new Email();
            return View(model);
        }

        public ActionResult Lobby()
        {
            ViewBag.Message = "Your Lobby page.";

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Contact(Email model)
        {
            if (ModelState.IsValid)
            {
                try
                {

                    var standardBodyMessage = $"<h3>You have received an email from {model.FromEmail}</h3>";
                    standardBodyMessage += $"<br /> {model.Body}";
                    var from = "Financial Enterprise<travwright3@gmail.com>";


                    var email = new MailMessage(from, ConfigurationManager.AppSettings["emailto"])
                    {
                        Subject = model.Subject,
                        Body = standardBodyMessage,
                        IsBodyHtml = true
                    };

                    var svc = new PersonalEmail();
                    await svc.SendAsync(email);

                    return RedirectToAction("Index", "BlogPosts");
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    await Task.FromResult(0);
                }
            }
            return View(model);
        }
    }
}