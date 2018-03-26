using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace HEJARITO.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit https://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        [Required]
        [DataType(DataType.Text)]
        [StringLength(100, ErrorMessage = "Förnamn måste bestå av minst {2} tecken.", MinimumLength = 2)]
        [Display(Name = "Förnamn")]
        public string FirstName { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [StringLength(100, ErrorMessage = "Efternamn måste bestå av minst {2} tecken.", MinimumLength = 2)]
        [Display(Name = "Efternamn")]
        public string LastName { get; set; }
        
        public int? CourseId { get; set; }
        public virtual Course Course { get; set; }

        public virtual ICollection<Document> Documents { get; set; }
        public virtual ICollection<StudentDocument> StudentDocuments { get; set; }


        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            //userIdentity.AddClaim(new Claim("FullName", this.FirstName +", " + this.LastName));
            
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

        public System.Data.Entity.DbSet<HEJARITO.Models.ActivityType> ActivityTypes { get; set; }

        public System.Data.Entity.DbSet<HEJARITO.Models.Activity> Activities { get; set; }

        public System.Data.Entity.DbSet<HEJARITO.Models.Module> Modules { get; set; }

        public System.Data.Entity.DbSet<HEJARITO.Models.Course> Courses { get; set; }

        public System.Data.Entity.DbSet<HEJARITO.Models.Document> Documents { get; set; }

        public System.Data.Entity.DbSet<HEJARITO.Models.StudentDocument> StudentDocuments { get; set; }

        public System.Data.Entity.DbSet<HEJARITO.Models.Feedback> Feedbacks { get; set; }

        //public System.Data.Entity.DbSet<HEJARITO.Models.ApplicationUser> ApplicationUsers { get; set; }
    }
} 