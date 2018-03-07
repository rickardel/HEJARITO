namespace HEJARITO.Migrations
{
    using HEJARITO.Models;
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<HEJARITO.Models.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(HEJARITO.Models.ApplicationDbContext context)
        {
            var roleStore = new RoleStore<IdentityRole>(context);
            var roleManager = new RoleManager<IdentityRole>(roleStore);

            var roleNames = new[] { Role.Teacher, Role.Student };
            foreach (var roleName in roleNames)
            {
                if (context.Roles.Any(r => r.Name == roleName)) continue;

                // Create role
                var role = new IdentityRole { Name = roleName };
                var result = roleManager.Create(role);
                if (!result.Succeeded)
                {
                    throw new Exception(string.Join("\n", result.Errors));
                }
            }

            var userStore = new UserStore<ApplicationUser>(context);
            var userManager = new UserManager<ApplicationUser>(userStore);

            var emails = new[] { "student@hejarito.se", "student2@hejarito.se", "student3@hejarito.se", "student4@hejarito.se", "teacher@hejarito.se", "teacher2@hejarito.se", "teacher3@hejarito.se" };
            foreach (var email in emails)
            {
                if (context.Users.Any(u => u.UserName == email)) continue;

                // Create user
                var user = new ApplicationUser { UserName = email, Email = email, FirstName = "Student", LastName = "Studentsson" };
                var result = userManager.Create(user, "password");
                if (!result.Succeeded)
                {
                    throw new Exception(string.Join("\n", result.Errors));
                }
                else
                {
                    if (user.Email.StartsWith("teacher")) {
                        user.FirstName = "Teacher";
                        userManager.AddToRole(user.Id, Role.Teacher);
                    }
                    else
                        userManager.AddToRole(user.Id, Role.Student);
                    
                }
            }


        }
    }
}
