using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace twright_FinacialPortal.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit https://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        
        public int? HouseholdId { get; set; }

        [StringLength(80, MinimumLength = 2, ErrorMessage = "Firstname must be between 2 and 80 characters long.")]
        public string FirstName { get; set; }

        [StringLength(80, MinimumLength = 2, ErrorMessage = "Lastname must be between 2 and 80 characters long.")]
        public string LastName { get; set; }


        [StringLength(80, ErrorMessage = "The Displayname must be at least 80 characters long.")]
        public string Nickname { get; set; }

        public string ProfilePic { get; set; }

        [NotMapped]
        public string FullName
        {
            get
            {
                return $"{LastName}, {FirstName}";
            }
            set { }

        }

        public virtual Household Household { get; set; }



        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            userIdentity.AddClaim(new Claim("HouseholdId", this.Household.ToString()));


           return userIdentity;
        }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

        public DbSet<BankAccount> BankAccounts { get; set; }
        public DbSet<BudgetCategory> BudgetCategories { get; set; }
        public DbSet<BudgetCategoryItem> BudgetCategoryItems { get; set; }
        public DbSet<Household> Households { get; set; }
        public DbSet<Invitation> Invitations { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<Transaction> Transactions { get; set; }
    }
}