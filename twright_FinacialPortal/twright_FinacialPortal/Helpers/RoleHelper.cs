﻿using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using twright_FinacialPortal.Enumerations;
using twright_FinacialPortal.Models;

namespace twright_FinacialPortal.Helpers
{
    public class RoleHelper
    {
        private UserManager<ApplicationUser> userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext()));
        private ApplicationDbContext db = new ApplicationDbContext();
        static ApplicationDbContext dB = new ApplicationDbContext();

        public bool IsUserInRole(string userId, string roleName)
        {
            return userManager.IsInRole(userId, roleName);
        }

        public bool IsEmailInRole(string email, string roleName)
        {
            var userId = userManager.FindByEmail(email).Id;
            return userManager.IsInRole(email, roleName);
        }

        public ICollection<string> ListUserRoles(string userId)
        {
            return userManager.GetRoles(userId);
        }

        public ICollection<string> ListEmailRoles(string email)
        {
            var userId = userManager.FindByEmail(email).Id;
            return userManager.GetRoles(userId);
        }

        public bool AddUserToRole(string userId, string roleName)
        {
            var result = userManager.AddToRole(userId, roleName);
            return result.Succeeded;
        }

        public bool RemoveUserFromRole(string userId, string roleName)
        {
            var result = userManager.RemoveFromRole(userId, roleName);
            return result.Succeeded;
        }

        public ICollection<ApplicationUser> UsersInRole(string roleName)
        {
            var resultList = new List<ApplicationUser>();
            var List = userManager.Users.ToList();
            foreach (var user in List)
            {
                if (IsUserInRole(user.Id, roleName))
                    resultList.Add(user);
            }
            return resultList;
        }

        public ICollection<ApplicationUser> UsersNotInRole(string roleName)
        {
            var resultList = new List<ApplicationUser>();
            var List = userManager.Users.ToList();
            foreach (var user in List)
            {
                if (!IsUserInRole(user.Id, roleName))
                    resultList.Add(user);
            }
            return resultList;
        }

        public static string GetUserImage(string userId)
        {
            return dB.Users.Find(userId).ProfilePic;
        }

        public bool AddUserToRole(string userId, PortalRole roleName)
        {
            var result = userManager.AddToRole(userId, roleName.ToString());
            return result.Succeeded;
        }

        public bool RemoveUserFromRole(string userId, PortalRole roleName)
        {
            var result = userManager.RemoveFromRole(userId, roleName.ToString());
            return result.Succeeded;
        }
    }
}