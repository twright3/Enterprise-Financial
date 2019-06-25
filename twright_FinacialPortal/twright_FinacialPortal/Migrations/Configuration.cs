namespace twright_FinacialPortal.Migrations
{
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using twright_FinacialPortal.Models;

    internal sealed class Configuration : DbMigrationsConfiguration<twright_FinacialPortal.Models.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
        }

        protected override void Seed(twright_FinacialPortal.Models.ApplicationDbContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data.
            var roleManager = new RoleManager<IdentityRole>(
                new RoleStore<IdentityRole>(context));

            if (!context.Roles.Any(r => r.Name == "HeadofHousehold"))
            {
                roleManager.Create(new IdentityRole { Name = "HeadofHousehold" });
            }

            if (!context.Roles.Any(r => r.Name == "Lobbyist"))
            {
                roleManager.Create(new IdentityRole { Name = "Lobbyist" });
            }
            if (!context.Roles.Any(r => r.Name == "Member"))
            {
                roleManager.Create(new IdentityRole { Name = "Member" });
            }
            if (!context.Roles.Any(r => r.Name == "Admin"))
            {
                roleManager.Create(new IdentityRole { Name = "Admin" });
            }

            context.Households.AddOrUpdate(
                t => t.Name,
                    new Household
                    {
                        Name = "The Lobby",
                        Greeting = "Hello Welcome To the Lobby",
                        Established = DateTime.Now,
                    },

                    new Household
                    {
                        Name = "Seed House",
                        Greeting = "Hello Welcome To Seed House",
                        Established = DateTime.Now,
                    },


                    new Household
                    {
                        Name = "Admin House",
                        Greeting = "Hello Welcome To Admin House",
                        Established = DateTime.Now,
                    });


            context.SaveChanges();

            var lobbyId = context.Households.AsNoTracking().FirstOrDefault(h => h.Name == "The Lobby").Id;
            var seedHouseId = context.Households.AsNoTracking().FirstOrDefault(h => h.Name == "Seed House").Id;
            var adminHouseId = context.Households.AsNoTracking().FirstOrDefault(h => h.Name == "Admin House").Id;

            //I want to write some code that allows me to introduce a few "roles" into our application
            var userManager = new UserManager<ApplicationUser>(
                new UserStore<ApplicationUser>(context));

            if (!context.Users.Any(u => u.Email == "travwright3@gmail.com"))
            {
                userManager.Create(new ApplicationUser
                {
                    Email = "travwright3@gmail.com",
                    UserName = "travwright3@gmail.com",
                    FirstName = "Travis",
                    LastName = "WrightHOH",
                    Nickname = "TWright",
                    ProfilePic = "/Avatar/default-user.jpg",
                    HouseholdId = seedHouseId
                }, "TKkhhW41010!");
            }

            var user = userManager.FindByEmail("travwright3@gmail.com");
            userManager.AddToRole(user.Id, "HeadofHousehold");

         

            //SeededResident

            if (!context.Users.Any(u => u.Email == "SeedLobbyist@Mailinator.com"))
            {
                userManager.Create(new ApplicationUser
                {
                    Email = "SeedLobbyist@Mailinator.com",
                    UserName = "SeedLobbyist@Mailinator.com",
                    FirstName = "Seed",
                    LastName = "Lobbyist",
                    Nickname = "Seed Lobbyist",
                    HouseholdId = lobbyId
                }, "TKkhhW41010!");
            }

            var userId = userManager.FindByEmail("SeedLobbyist@Mailinator.com").Id;
            userManager.AddToRole(userId, "Lobbyist");

            //SeededMember

            if (!context.Users.Any(u => u.Email == "SeedMember@Mailinator.com"))
            {
                userManager.Create(new ApplicationUser
                {
                    Email = "SeedMember@Mailinator.com",
                    UserName = "SeedMember@Mailinator.com",
                    FirstName = "Seed",
                    LastName = "Member",
                    Nickname = "Seed Member",
                    HouseholdId = seedHouseId
                }, "TKkhhW41010!");
            }

            userId = userManager.FindByEmail("SeedMember@Mailinator.com").Id;
            userManager.AddToRole(userId, "Member");

            //Admin
            if (!context.Users.Any(u => u.Email == "traviswright2@gmail.com"))
            {
                userManager.Create(new ApplicationUser
                {
                    Email = "traviswright2@gmail.com",
                    UserName = "traviswright2@gmail.com",
                    FirstName = "Travis",
                    LastName = "WrightAdmin",
                    Nickname = "AdminWright",
                    ProfilePic = "/Avatar/default-user.jpg",
                    HouseholdId = adminHouseId
                }, "TKkhhW41010!");
            }

            userId = userManager.FindByEmail("traviswright2@gmail.com").Id;
            userManager.AddToRole(user.Id, "Admin");

        }
    }
}
