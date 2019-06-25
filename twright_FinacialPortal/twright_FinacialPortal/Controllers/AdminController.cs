using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using twright_FinacialPortal.Helpers;
using twright_FinacialPortal.Models;
using twright_FinacialPortal.ViewModels;
using twright_FinacialPortal.ExtensionMethods;

namespace twright_FinacialPortal.Controllers
{
    public class AdminController : Controller
    {
        //I want to add a class level property that can access the BD
        private ApplicationDbContext db = new ApplicationDbContext();



        //I want to add a class level property that can help me namage Roles
        private RoleHelper roleHelper = new RoleHelper();

        [Authorize]
        public ActionResult Dashboard()
        {
            var userId = User.Identity.GetUserId();
            var currentUser = db.Users.Find(userId);
            //ask where this user is an admin or not
            //if(currentUser.IsHOH())
            //{
            //}
            //if (currentUser.OccupiesRole("HeadofHouseHold"))

            var currentRole = currentUser.GetRole();
            switch(currentRole)
            {
                case "Admin":
                    return View("Dashboard", "Admin");
                case "HeadofHousehold":
                case "Member":
                    return RedirectToAction("Dashboard", "HouseHolds");
                case "Lobbyist":
                    return RedirectToAction("Lobby", "Home");
            }

            return View();
        }

        // GET: Admin
        public ActionResult Index()
        {
            return View();
        }



        

        // GET: Admin
        [Authorize(Roles = "Admin")]
        public ActionResult ManageRoles(string userId)
        {

            //ViewBag.Users = new SelectList(db.Users.ToList(), "Id", "Email");
            ViewBag.UserId = userId;
            var currentRole = roleHelper.ListUserRoles(userId).FirstOrDefault();
            ViewBag.Roles = new SelectList(db.Roles.ToList(), "Name", "Name", currentRole);
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ManageRoles(string userId, string roles)
        {
            //I want to ensure theat the person I selected occupies one and only 1 role
            //Therefore the first thing I am going to do is remove the user from any role
            //they currently occupy



            //Step 1: Remove any roles currently occupied by the user. We can do this by
            // looping over the roles currently occupied by the user using the roleHelper
            foreach (var role in roleHelper.ListUserRoles(userId))
            {
                roleHelper.RemoveUserFromRole(userId, role);
            }

            //Step 2: Add the newly selected role to the user
            roleHelper.AddUserToRole(userId, roles);

            return RedirectToAction("UserIndex", "Admin");
        }

        [Authorize]
        public ActionResult ManageMyRole(string email)
        {
            //I want to determine if I am already in role...

            var myCurrentRole = roleHelper.ListEmailRoles(email).FirstOrDefault();

            ViewBag.Roles = new SelectList(db.Roles.ToList(), "Name", "Name", myCurrentRole);
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ManageMyRole(string email, string roles)
        {
            var userId = db.Users.FirstOrDefault(u => u.Email == email).Id;
            foreach (var role in roleHelper.ListUserRoles(userId))
            {
                roleHelper.RemoveUserFromRole(userId, roles);
            }


            //Step 2: Add the newly selected role to the user
            roleHelper.AddUserToRole(userId, roles);

            return RedirectToAction("UserIndex", "Admin");
        }


        public ActionResult UserIndex()
        {
            var users = db.Users.Select(u => new UserViewModel
            {
                Id = u.Id,
                FirstName = u.FirstName,
                LastName = u.LastName,
                Nickname = u.Nickname,
                Email = u.Email
            }).ToList();


            return View(users);
        }

    }
}