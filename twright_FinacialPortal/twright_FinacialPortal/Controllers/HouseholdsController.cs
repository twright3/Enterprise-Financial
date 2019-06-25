using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using twright_FinacialPortal.Models;
using twright_FinacialPortal.Helpers;
using twright_FinacialPortal.Enumerations;
using System.Threading.Tasks;
using twright_FinacialPortal.ExtensionMethods;

namespace twright_FinacialPortal.Controllers
{
    public class HouseholdsController : Controller
    {


        public ActionResult Dashboard()
        {
            var userId = User.Identity.GetUserId();
            var houseId = db.Users.Find(userId).HouseholdId;

            return View(db.Households.AsNoTracking().FirstOrDefault(h => h.Id == houseId));
        }

        private ApplicationDbContext db = new ApplicationDbContext();
        public RoleHelper roleHelper = new RoleHelper();

        // GET: Households
        public ActionResult Index()
        {
            var households = db.Households;
            return View(households.ToList());
        }

        // GET: Households/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Household household = db.Households.Find(id);
            if (household == null)
            {
                return HttpNotFound();
            }
            return View(household);
        }

        // GET: Households/Create
        public ActionResult Create()
        {
            //ViewBag.HeadofHouseholdId = new SelectList(db.Users, "Id", "FirstName");
            return View();
        }

        // POST: Households/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,Name,Greeting")] Household household)
        {
            if (ModelState.IsValid)
            {
                

                household.Established = DateTime.Now;
                db.Households.Add(household);

                var userId = User.Identity.GetUserId();
                var user = db.Users.Find(userId);
                user.HouseholdId = household.Id;
                db.SaveChanges();

                roleHelper.RemoveUserFromRole(userId, PortalRole.Lobbyist);
                roleHelper.AddUserToRole(userId, PortalRole.HeadofHousehold);

                await AdminHelper.ReauthorizeUserAsync(userId);

                return RedirectToAction("Dashboard");
            }

            
            return View(household);
        }

        // GET: Households/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Household household = db.Households.Find(id);
            if (household == null)
            {
                return HttpNotFound();
            }
            
            return View(household);
        }

        // POST: Households/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,Greeting,Established,HeadofHouseholdId")] Household household)
        {
            if (ModelState.IsValid)
            {
                db.Entry(household).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            
            return View(household);
        }

        // GET: Households/Delete/5
        //public ActionResult Delete(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    Household household = db.Households.Find(id);
        //    if (household == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(household);
        //}

        // POST: Households/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public ActionResult DeleteConfirmed(int id)
        //{
        //    Household household = db.Households.Find(id);
        //    db.Households.Remove(household);
        //    db.SaveChanges();
        //    return RedirectToAction("Index");
        //}

        [Authorize]
        public ActionResult InviteMember()
        {
            
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> InviteMember(Invitation invitation)
        {
            invitation.IsValid = true;
            invitation.HouseholdId = User.Identity.GetHouseholdId();
            invitation.Created = DateTime.Now;
            invitation.Code = Guid.NewGuid();

            db.Invitations.Add(invitation);
            db.SaveChanges();

            await invitation.SendAsync();

            return RedirectToAction("Dashboard","Household");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }


        // Leave a household attempt.
        public ActionResult LeaveHousehold(string userId)
        {
            var user = db.Users.Find(userId);
            user.HouseholdId = db.Households.FirstOrDefault(h => h.Name == "The Lobby").Id;
            roleHelper.RemoveUserFromRole(userId, PortalRole.Member);
            roleHelper.AddUserToRole(userId, PortalRole.Lobbyist);
            db.SaveChanges();


            // looping over the roles currently occupied by the user using the roleHelper
            //foreach (var role in roleHelper.ListUserRoles(userId))
            //{
            //    roleHelper.RemoveUserFromRole(userId, role);
            //}

            ////Step 2: Add the newly selected role to the user
            //roleHelper.AddUserToRole(userId, "Lobbyist");

            ////how to have HOH to leave but first give HOH to someone else

            return RedirectToAction("Lobby", "Home");
        }
    }
}
