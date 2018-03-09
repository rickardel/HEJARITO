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

            //TM 2018-03-08 11:26 Seeda Courses

            var courses = new[] {
                new Course { Name = "IT-teknik + Office 365 #2018/1",               StartDate = DateTime.Parse("2018-04-01"), EndDate = DateTime.Parse("2018-07-31") },
                new Course { Name = "Systemutveckling i Java #2018/1",              StartDate = DateTime.Parse("2018-04-10"), EndDate = DateTime.Parse("2018-08-31") },
                new Course { Name = "Systemutveckling i ASP.NET MVC/C# #2018/1",    StartDate = DateTime.Parse("2018-04-20"), EndDate = DateTime.Parse("2018-09-30") },
                new Course { Name = "Systemutveckling i ASP.NET MVC/C# #2018/2",    StartDate = DateTime.Parse("2018-05-20"), EndDate = DateTime.Parse("2018-10-31") },
            };

            context.Courses.AddOrUpdate(
                unik => unik.Name,
                courses
                );

            context.SaveChanges();

            //TM 2018-03-08 11:54 Seeda Modules

            var modules = new[] {
            // Moduler inom courses[0]
                new Module { Name = "Inledning (IT)",       CourseId = courses[0].Id, StartDate = courses[0].StartDate,             EndDate = courses[0].StartDate},
                new Module { Name = "Windows Server 2016",  CourseId = courses[0].Id, StartDate = courses[0].StartDate.AddDays(1),  EndDate = courses[0].StartDate.AddDays(10) },
                new Module { Name = "SQL Server 2017 (IT)", CourseId = courses[0].Id, StartDate = courses[0].StartDate.AddDays(11), EndDate = courses[0].StartDate.AddDays(20)  },
                new Module { Name = "Office 365",           CourseId = courses[0].Id, StartDate = courses[0].StartDate.AddDays(21), EndDate = courses[0].StartDate.AddDays(30)  },

            // Moduler inom courses[1]
                new Module { Name = "Inledning (Java)",                     CourseId = courses[1].Id, StartDate = courses[1].StartDate,             EndDate = courses[1].StartDate},
                new Module { Name = "Objektorienteringens grunder (Java)",  CourseId = courses[1].Id, StartDate = courses[1].StartDate.AddDays(1),  EndDate = courses[1].StartDate.AddDays(10) },
                new Module { Name = "Oracle Database 12c Release 2",        CourseId = courses[1].Id, StartDate = courses[1].StartDate.AddDays(11), EndDate = courses[1].StartDate.AddDays(20)  },
                new Module { Name = "Systemutveckling i Java",              CourseId = courses[1].Id, StartDate = courses[1].StartDate.AddDays(21), EndDate = courses[1].StartDate.AddDays(30)  },

            // Moduler inom courses[2]
                new Module { Name = "Inledning (MVC#1)",                    CourseId = courses[2].Id, StartDate = courses[2].StartDate,             EndDate = courses[2].StartDate},
                new Module { Name = "Objektorienteringens grunder (MVC#1)", CourseId = courses[2].Id, StartDate = courses[2].StartDate.AddDays(1),  EndDate = courses[2].StartDate.AddDays(10) },
                new Module { Name = "SQL Server 2017 (MVC#1)",              CourseId = courses[2].Id, StartDate = courses[2].StartDate.AddDays(11), EndDate = courses[2].StartDate.AddDays(20)  },
                new Module { Name = "Systemutveckling i ASP.NET (MVC#1)",   CourseId = courses[2].Id, StartDate = courses[2].StartDate.AddDays(21), EndDate = courses[2].StartDate.AddDays(30)  },

            // Moduler inom courses[3]
                new Module { Name = "Inledning (MVC#2)",                    CourseId = courses[3].Id, StartDate = courses[3].StartDate,             EndDate = courses[3].StartDate},
                new Module { Name = "Objektorienteringens grunder (MVC#2)", CourseId = courses[3].Id, StartDate = courses[3].StartDate.AddDays(1),  EndDate = courses[3].StartDate.AddDays(10) },
                new Module { Name = "SQL Server 2017 (MVC#2)",              CourseId = courses[3].Id, StartDate = courses[3].StartDate.AddDays(11), EndDate = courses[3].StartDate.AddDays(20)  },
                new Module { Name = "Systemutveckling i ASP.NET (MVC#2)",   CourseId = courses[3].Id, StartDate = courses[3].StartDate.AddDays(21), EndDate = courses[3].StartDate.AddDays(30)  }
            };

            context.Modules.AddOrUpdate(
                unik => unik.Name,
                modules
                );

            context.SaveChanges();

            //TM 2018-03-08 10:58 Seeda ActivityTypes

            var activitytypes = new[] {
                new ActivityType { Name = "Föreläsning" },
                new ActivityType { Name = "e-Learning" },
                new ActivityType { Name = "Övningstillfälle" },
                new ActivityType { Name = "annat" }
            };

            context.ActivityTypes.AddOrUpdate(
                unik => unik.Name,
                activitytypes
                );

            context.SaveChanges();

            //TM 2018-03-08 13:05 Seeda Activities
            //OBS!!! DeadlineDate ska också ha något värde, annars kraschar appen med konferteringsfel DateTime -> DateTime2 p.g.a. DateTime är en struct och kan därför inte vara null!

            var activities = new[] {

            // Inledningsmodulernas enda aktivitet (modules[0], [4], [8], [12])
                new Activity { ModuleId = modules[0].Id,    Name = "Inledande infomöte (IT)",       ActivityTypeId = activitytypes[3].Id, Description = "OBS!!! Obligatorisk startaktivitet för varje kurs!", StartDate = modules[0].StartDate,     DeadlineDate = modules[0].StartDate,    EndDate = modules[0].StartDate },
                new Activity { ModuleId = modules[4].Id,    Name = "Inledande infomöte (Java)",     ActivityTypeId = activitytypes[3].Id, Description = "OBS!!! Obligatorisk startaktivitet för varje kurs!", StartDate = modules[4].StartDate,     DeadlineDate = modules[4].StartDate,    EndDate = modules[4].StartDate },
                new Activity { ModuleId = modules[8].Id,    Name = "Inledande infomöte (MVC#1)",    ActivityTypeId = activitytypes[3].Id, Description = "OBS!!! Obligatorisk startaktivitet för varje kurs!", StartDate = modules[8].StartDate,     DeadlineDate = modules[8].StartDate,    EndDate = modules[8].StartDate },
                new Activity { ModuleId = modules[12].Id,   Name = "Inledande infomöte (MVC#2)",    ActivityTypeId = activitytypes[3].Id, Description = "OBS!!! Obligatorisk startaktivitet för varje kurs!", StartDate = modules[12].StartDate,    DeadlineDate = modules[12].StartDate,   EndDate = modules[12].StartDate },

            // Windows Server modulens aktiviteter
                new Activity { ModuleId = modules[1].Id,    Name = "Teori (WinServ)",   ActivityTypeId = activitytypes[1].Id,   StartDate = modules[1].StartDate,               EndDate = modules[1].StartDate.AddDays(1),  DeadlineDate = modules[1].StartDate.AddDays(1) },
                new Activity { ModuleId = modules[1].Id,    Name = "Övning (WinServ)",  ActivityTypeId = activitytypes[2].Id,   StartDate = modules[1].StartDate.AddDays(2),    EndDate = modules[1].StartDate.AddDays(5),  DeadlineDate = modules[1].StartDate.AddDays(5)  },

            // SQL Server modulens aktiviteter i courses[0], [2], [3]
                new Activity { ModuleId = modules[2].Id,    Name = "Teori   (SQLServ/IT)",  ActivityTypeId = activitytypes[0].Id,   StartDate = modules[2].StartDate,               EndDate = modules[2].StartDate.AddDays(1), DeadlineDate = modules[2].StartDate.AddDays(1) },
                new Activity { ModuleId = modules[2].Id,    Name = "Övning  (SQLServ/IT)",  ActivityTypeId = activitytypes[2].Id,   StartDate = modules[2].StartDate.AddDays(2),    EndDate = modules[2].StartDate.AddDays(5), DeadlineDate = modules[2].StartDate.AddDays(5) },

                new Activity { ModuleId = modules[10].Id,    Name = "Teori  (SQLServ/MVC#1)",   ActivityTypeId = activitytypes[1].Id,   StartDate = modules[10].StartDate,              EndDate = modules[10].StartDate.AddDays(1), DeadlineDate = modules[10].StartDate.AddDays(1) },
                new Activity { ModuleId = modules[10].Id,    Name = "Övning (SQLServ/MVC#1)",   ActivityTypeId = activitytypes[2].Id,   StartDate = modules[10].StartDate.AddDays(2),   EndDate = modules[10].StartDate.AddDays(5), DeadlineDate = modules[10].StartDate.AddDays(5) },

                new Activity { ModuleId = modules[14].Id,    Name = "Teori  (SQLServ/MVC#2)",   ActivityTypeId = activitytypes[1].Id,   StartDate = modules[14].StartDate,              EndDate = modules[14].StartDate.AddDays(1), DeadlineDate = modules[14].StartDate.AddDays(1) },
                new Activity { ModuleId = modules[14].Id,    Name = "Övning (SQLServ/MVC#2)",   ActivityTypeId = activitytypes[2].Id,   StartDate = modules[14].StartDate.AddDays(2),   EndDate = modules[14].StartDate.AddDays(5), DeadlineDate = modules[14].StartDate.AddDays(5) },

            // Office 365 modulens aktiviteter 
                new Activity { ModuleId = modules[3].Id,    Name = "Teori (Office)",    ActivityTypeId = activitytypes[0].Id,   StartDate = modules[3].StartDate,               EndDate = modules[3].StartDate.AddDays(1), DeadlineDate = modules[3].StartDate.AddDays(1) },
                new Activity { ModuleId = modules[3].Id,    Name = "Övning (Office)",   ActivityTypeId = activitytypes[2].Id,   StartDate = modules[3].StartDate.AddDays(2),    EndDate = modules[3].StartDate.AddDays(5), DeadlineDate = modules[3].StartDate.AddDays(5) },

            // OO-modulens aktiviteter i courses[1], [2], [3]
                new Activity { ModuleId = modules[5].Id,    Name = "Teori   (OO/Java)", ActivityTypeId = activitytypes[1].Id,   StartDate = modules[5].StartDate,               EndDate = modules[5].StartDate.AddDays(1),  DeadlineDate = modules[5].StartDate.AddDays(1) },
                new Activity { ModuleId = modules[5].Id,    Name = "Övning  (OO/Java)", ActivityTypeId = activitytypes[2].Id,   StartDate = modules[5].StartDate.AddDays(2),    EndDate = modules[5].StartDate.AddDays(5),  DeadlineDate = modules[5].StartDate.AddDays(5) },

                new Activity { ModuleId = modules[9].Id,   Name = "Teori   (OO/MVC#1)",    ActivityTypeId = activitytypes[0].Id,   StartDate = modules[9].StartDate,               EndDate = modules[9].StartDate.AddDays(1), DeadlineDate = modules[9].StartDate.AddDays(1) },
                new Activity { ModuleId = modules[9].Id,   Name = "Övning  (OO/MVC#1)",    ActivityTypeId = activitytypes[2].Id,   StartDate = modules[9].StartDate.AddDays(2),    EndDate = modules[9].StartDate.AddDays(5), DeadlineDate = modules[9].StartDate.AddDays(5) },

                new Activity { ModuleId = modules[13].Id,   Name = "Teori   (OO/MVC#2)",    ActivityTypeId = activitytypes[1].Id,   StartDate = modules[13].StartDate,              EndDate = modules[13].StartDate.AddDays(1), DeadlineDate = modules[13].StartDate.AddDays(1) },
                new Activity { ModuleId = modules[13].Id,   Name = "Övning  (OO/MVC#2)",    ActivityTypeId = activitytypes[2].Id,   StartDate = modules[13].StartDate.AddDays(2),   EndDate = modules[13].StartDate.AddDays(5), DeadlineDate = modules[13].StartDate.AddDays(5) },

            // Oracle-modulens aktiviteter 
                new Activity { ModuleId = modules[6].Id,    Name = "Teori   (Oracle)",  ActivityTypeId = activitytypes[0].Id,   StartDate = modules[6].StartDate,               EndDate = modules[6].StartDate.AddDays(1),  DeadlineDate = modules[6].StartDate.AddDays(1) },
                new Activity { ModuleId = modules[6].Id,    Name = "Övning  (Oracle)",  ActivityTypeId = activitytypes[2].Id,   StartDate = modules[6].StartDate.AddDays(2),    EndDate = modules[6].StartDate.AddDays(5),  DeadlineDate = modules[6].StartDate.AddDays(5) },

            // Java-modulens aktiviteter 
                new Activity { ModuleId = modules[7].Id,    Name = "Teori   (Java)",    ActivityTypeId = activitytypes[1].Id,   StartDate = modules[7].StartDate,               EndDate = modules[7].StartDate.AddDays(1),  DeadlineDate = modules[7].StartDate.AddDays(1) },
                new Activity { ModuleId = modules[7].Id,    Name = "Övning  (Java)",    ActivityTypeId = activitytypes[2].Id,   StartDate = modules[7].StartDate.AddDays(2),    EndDate = modules[7].StartDate.AddDays(5),  DeadlineDate = modules[7].StartDate.AddDays(5) },

            // MVC-modulens aktiviteter (courses [2], [3])
                new Activity { ModuleId = modules[11].Id,    Name = "Teori  (MVC#1)",   ActivityTypeId = activitytypes[0].Id,   StartDate = modules[11].StartDate,              EndDate = modules[11].StartDate.AddDays(1), DeadlineDate = modules[11].StartDate.AddDays(1) },
                new Activity { ModuleId = modules[11].Id,    Name = "Övning (MVC#1)",   ActivityTypeId = activitytypes[2].Id,   StartDate = modules[11].StartDate.AddDays(2),   EndDate = modules[11].StartDate.AddDays(5), DeadlineDate = modules[11].StartDate.AddDays(5) },

                new Activity { ModuleId = modules[15].Id,    Name = "Teori  (MVC#2)",   ActivityTypeId = activitytypes[1].Id,   StartDate = modules[15].StartDate,              EndDate = modules[15].StartDate.AddDays(1), DeadlineDate = modules[15].StartDate.AddDays(1) },
                new Activity { ModuleId = modules[15].Id,    Name = "Övning (MVC#2)",   ActivityTypeId = activitytypes[2].Id,   StartDate = modules[15].StartDate.AddDays(2),   EndDate = modules[15].StartDate.AddDays(5), DeadlineDate = modules[15].StartDate.AddDays(5) }
            };


            context.Activities.AddOrUpdate(
                unik => unik.Name,
                activities
                );

            context.SaveChanges();
        }
    }
}
