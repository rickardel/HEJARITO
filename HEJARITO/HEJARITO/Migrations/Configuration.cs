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

            //TM 2018-03-08 11:26 Seeda Courses
            //TM 2018-03-13 10:07 Utökades med 3 st #2018/0-kurser
            //TM 2018-03-13 11:26 Kurser ska seedas före elever för att sistnämnda ska kunna tilldelas någon av de förstnämnda

            var courses = new[] {
                new Course { Name = "IT-teknik + Office 365 #2018/0",               StartDate = DateTime.Parse("2018-03-01"), EndDate = DateTime.Parse("2018-06-15") },
                new Course { Name = "Systemutveckling i Java #2018/0",              StartDate = DateTime.Parse("2018-03-10"), EndDate = DateTime.Parse("2018-07-16") },
                new Course { Name = "Systemutveckling i ASP.NET MVC/C# #2018/0",    StartDate = DateTime.Parse("2018-03-20"), EndDate = DateTime.Parse("2018-08-17") },
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

            //TM 2018-03-13 11:28 Här börjar seedning av användare (30 elever + 3 lärare)

            var emails = new[] {
                "student@hejarito.se",
                "student2@hejarito.se",
                "student3@hejarito.se",
                "student4@hejarito.se",

                //TM 2018-03-13 11:22 Ytterligare 26 elever ska seedas...
                "student5@hejarito.se",
                "student6@hejarito.se",
                "student7@hejarito.se",
                "student8@hejarito.se",
                "student9@hejarito.se",
                "student10@hejarito.se",
                "student11@hejarito.se",
                "student12@hejarito.se",
                "student13@hejarito.se",
                "student14@hejarito.se",
                "student15@hejarito.se",
                "student16@hejarito.se",
                "student17@hejarito.se",
                "student18@hejarito.se",
                "student19@hejarito.se",
                "student20@hejarito.se",
                "student21@hejarito.se",
                "student22@hejarito.se",
                "student23@hejarito.se",
                "student24@hejarito.se",
                "student25@hejarito.se",
                "student26@hejarito.se",
                "student27@hejarito.se",
                "student28@hejarito.se",
                "student29@hejarito.se",
                "student30@hejarito.se",

                //TM 2018-03-13 11:22 ... samt 3 lärare
                "teacher@hejarito.se",
                "teacher2@hejarito.se",
                "teacher3@hejarito.se" };

            int n = 0;

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
                        user.FirstName  = "Teacher";
                        user.LastName   = "Teachersson";
                        userManager.AddToRole(user.Id, Role.Teacher);
                    }
                    else
                    {
                        userManager.AddToRole(user.Id, Role.Student);

                        //TM 2018-03-13 Här tilldelas eleven en kurs
                        user.CourseId = courses[n % courses.Count()].Id;
                        ++n;
                    }
                }
            }

            //TM 2018-03-08 11:54 Seeda Modules
            //TM 2018-03-13 10:12 Utökades med moduler tillhörandes de 3 nya kurserna

            var modules = new[] {
            // Moduler inom courses[0]
                new Module { Name = "Inledning (IT#0)",             CourseId = courses[0].Id, StartDate = courses[0].StartDate,             EndDate = courses[0].StartDate},
                new Module { Name = "Windows Server 2016 (IT#0)",   CourseId = courses[0].Id, StartDate = courses[0].StartDate.AddDays(1),  EndDate = courses[0].StartDate.AddDays(10) },
                new Module { Name = "SQL Server 2017 (IT#0)",       CourseId = courses[0].Id, StartDate = courses[0].StartDate.AddDays(11), EndDate = courses[0].StartDate.AddDays(20)  },
                new Module { Name = "Office 365 (IT#0)",            CourseId = courses[0].Id, StartDate = courses[0].StartDate.AddDays(21), EndDate = courses[0].StartDate.AddDays(30)  },

            // Moduler inom courses[1]
                new Module { Name = "Inledning (Java#0)",                       CourseId = courses[1].Id, StartDate = courses[1].StartDate,             EndDate = courses[1].StartDate},
                new Module { Name = "Objektorienteringens grunder (Java#0)",    CourseId = courses[1].Id, StartDate = courses[1].StartDate.AddDays(1),  EndDate = courses[1].StartDate.AddDays(10) },
                new Module { Name = "Oracle Database 12c Release 2 (Java#0)",   CourseId = courses[1].Id, StartDate = courses[1].StartDate.AddDays(11), EndDate = courses[1].StartDate.AddDays(20)  },
                new Module { Name = "Systemutveckling i Java (Java#0)",         CourseId = courses[1].Id, StartDate = courses[1].StartDate.AddDays(21), EndDate = courses[1].StartDate.AddDays(30)  },

            // Moduler inom courses[2]
                new Module { Name = "Inledning (MVC#0)",                    CourseId = courses[2].Id, StartDate = courses[2].StartDate,             EndDate = courses[2].StartDate},
                new Module { Name = "Objektorienteringens grunder (MVC#0)", CourseId = courses[2].Id, StartDate = courses[2].StartDate.AddDays(1),  EndDate = courses[2].StartDate.AddDays(10) },
                new Module { Name = "SQL Server 2017 (MVC#0)",              CourseId = courses[2].Id, StartDate = courses[2].StartDate.AddDays(11), EndDate = courses[2].StartDate.AddDays(20)  },
                new Module { Name = "Systemutveckling i ASP.NET (MVC#0)",   CourseId = courses[2].Id, StartDate = courses[2].StartDate.AddDays(21), EndDate = courses[2].StartDate.AddDays(30)  },

            // Moduler inom courses[3]
                new Module { Name = "Inledning (IT#1)",             CourseId = courses[3].Id, StartDate = courses[3].StartDate,             EndDate = courses[3].StartDate},
                new Module { Name = "Windows Server 2016 (IT#1)",   CourseId = courses[3].Id, StartDate = courses[3].StartDate.AddDays(1),  EndDate = courses[3].StartDate.AddDays(10) },
                new Module { Name = "SQL Server 2017 (IT#1)",       CourseId = courses[3].Id, StartDate = courses[3].StartDate.AddDays(11), EndDate = courses[3].StartDate.AddDays(20)  },
                new Module { Name = "Office 365 (IT#1)",            CourseId = courses[3].Id, StartDate = courses[3].StartDate.AddDays(21), EndDate = courses[3].StartDate.AddDays(30)  },

            // Moduler inom courses[4]
                new Module { Name = "Inledning (Java#1)",                       CourseId = courses[4].Id, StartDate = courses[4].StartDate,             EndDate = courses[4].StartDate},
                new Module { Name = "Objektorienteringens grunder (Java#1)",    CourseId = courses[4].Id, StartDate = courses[4].StartDate.AddDays(1),  EndDate = courses[4].StartDate.AddDays(10) },
                new Module { Name = "Oracle Database 12c Release 2 (Java#1)",   CourseId = courses[4].Id, StartDate = courses[4].StartDate.AddDays(11), EndDate = courses[4].StartDate.AddDays(20)  },
                new Module { Name = "Systemutveckling i Java (Java#1)",         CourseId = courses[4].Id, StartDate = courses[4].StartDate.AddDays(21), EndDate = courses[4].StartDate.AddDays(30)  },

            // Moduler inom courses[5]
                new Module { Name = "Inledning (MVC#1)",                    CourseId = courses[5].Id, StartDate = courses[5].StartDate,             EndDate = courses[5].StartDate},
                new Module { Name = "Objektorienteringens grunder (MVC#1)", CourseId = courses[5].Id, StartDate = courses[5].StartDate.AddDays(1),  EndDate = courses[5].StartDate.AddDays(10) },
                new Module { Name = "SQL Server 2017 (MVC#1)",              CourseId = courses[5].Id, StartDate = courses[5].StartDate.AddDays(11), EndDate = courses[5].StartDate.AddDays(20)  },
                new Module { Name = "Systemutveckling i ASP.NET (MVC#1)",   CourseId = courses[5].Id, StartDate = courses[5].StartDate.AddDays(21), EndDate = courses[5].StartDate.AddDays(30)  },

            // Moduler inom courses[6]
                new Module { Name = "Inledning (MVC#2)",                    CourseId = courses[6].Id, StartDate = courses[6].StartDate,             EndDate = courses[6].StartDate},
                new Module { Name = "Objektorienteringens grunder (MVC#2)", CourseId = courses[6].Id, StartDate = courses[6].StartDate.AddDays(1),  EndDate = courses[6].StartDate.AddDays(10) },
                new Module { Name = "SQL Server 2017 (MVC#2)",              CourseId = courses[6].Id, StartDate = courses[6].StartDate.AddDays(11), EndDate = courses[6].StartDate.AddDays(20)  },
                new Module { Name = "Systemutveckling i ASP.NET (MVC#2)",   CourseId = courses[6].Id, StartDate = courses[6].StartDate.AddDays(21), EndDate = courses[6].StartDate.AddDays(30)  }
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
            //TM 2018-03-13 11:02 Utökades med aktiviteter tillhörandes modulerna i de 3 nya kurserna
            //OBS!!! DeadlineDate är nu av typ Nullable<DateTime>

            var activities = new[] {

            // Inledningsmodulernas enda aktivitet (modules[0], [4], [8], [12], [16], [20], [24])
                new Activity { ModuleId = modules[0].Id,    Name = "Inledande infomöte (IT#0)",     ActivityTypeId = activitytypes[3].Id, Description = "OBS!!! Obligatorisk startaktivitet för varje kurs!",   StartDate = modules[0].StartDate,   DeadlineDate = modules[0].EndDate,  EndDate = modules[0].EndDate},
                new Activity { ModuleId = modules[4].Id,    Name = "Inledande infomöte (Java#0)",   ActivityTypeId = activitytypes[3].Id, Description = "OBS!!! Obligatorisk startaktivitet för varje kurs!",   StartDate = modules[4].StartDate,   DeadlineDate = modules[4].EndDate,  EndDate = modules[4].EndDate},
                new Activity { ModuleId = modules[8].Id,    Name = "Inledande infomöte (MVC#0)",    ActivityTypeId = activitytypes[3].Id, Description = "OBS!!! Obligatorisk startaktivitet för varje kurs!",   StartDate = modules[8].StartDate,   DeadlineDate = modules[8].EndDate,  EndDate = modules[8].EndDate},
                new Activity { ModuleId = modules[12].Id,   Name = "Inledande infomöte (IT#1)",     ActivityTypeId = activitytypes[3].Id, Description = "OBS!!! Obligatorisk startaktivitet för varje kurs!",   StartDate = modules[12].StartDate,  DeadlineDate = modules[12].EndDate, EndDate = modules[12].EndDate},
                new Activity { ModuleId = modules[16].Id,   Name = "Inledande infomöte (Java#1)",   ActivityTypeId = activitytypes[3].Id, Description = "OBS!!! Obligatorisk startaktivitet för varje kurs!",   StartDate = modules[16].StartDate,  DeadlineDate = modules[16].EndDate, EndDate = modules[16].EndDate},
                new Activity { ModuleId = modules[20].Id,   Name = "Inledande infomöte (MVC#1)",    ActivityTypeId = activitytypes[3].Id, Description = "OBS!!! Obligatorisk startaktivitet för varje kurs!",   StartDate = modules[20].StartDate,  DeadlineDate = modules[20].EndDate, EndDate = modules[20].EndDate},
                new Activity { ModuleId = modules[24].Id,   Name = "Inledande infomöte (MVC#2)",    ActivityTypeId = activitytypes[3].Id, Description = "OBS!!! Obligatorisk startaktivitet för varje kurs!",   StartDate = modules[24].StartDate,  DeadlineDate = modules[24].EndDate, EndDate = modules[24].EndDate},

            // Windows Server modulens aktiviteter (modules[1], [13])
                new Activity { ModuleId = modules[1].Id,    Name = "Teori (WinServ#0)",     ActivityTypeId = activitytypes[1].Id,   StartDate = modules[1].StartDate,             EndDate = modules[1].StartDate.AddDays(1),  DeadlineDate = modules[1].StartDate.AddDays(1) },
                new Activity { ModuleId = modules[1].Id,    Name = "Övning (WinServ#0)",    ActivityTypeId = activitytypes[2].Id,   StartDate = modules[1].StartDate.AddDays(2),  EndDate = modules[1].StartDate.AddDays(5),  DeadlineDate = modules[1].StartDate.AddDays(5)  },
                new Activity { ModuleId = modules[13].Id,   Name = "Teori (WinServ#1)",     ActivityTypeId = activitytypes[1].Id,   StartDate = modules[13].StartDate,            EndDate = modules[13].StartDate.AddDays(1), DeadlineDate = modules[13].StartDate.AddDays(1) },
                new Activity { ModuleId = modules[13].Id,   Name = "Övning (WinServ#1)",    ActivityTypeId = activitytypes[2].Id,   StartDate = modules[13].StartDate.AddDays(2), EndDate = modules[13].StartDate.AddDays(5), DeadlineDate = modules[13].StartDate.AddDays(5)  },

            // SQL Server modulens aktiviteter (modules[2], [10], [14], [22], [26])
                new Activity { ModuleId = modules[2].Id,    Name = "Teori   (SQLServ/IT#0)",    ActivityTypeId = activitytypes[0].Id,   StartDate = modules[2].StartDate,               EndDate = modules[2].StartDate.AddDays(1), DeadlineDate = modules[2].StartDate.AddDays(1) },
                new Activity { ModuleId = modules[2].Id,    Name = "Övning  (SQLServ/IT#0)",    ActivityTypeId = activitytypes[2].Id,   StartDate = modules[2].StartDate.AddDays(2),    EndDate = modules[2].StartDate.AddDays(5), DeadlineDate = modules[2].StartDate.AddDays(5) },

                new Activity { ModuleId = modules[10].Id,   Name = "Teori  (SQLServ/MVC#0)",    ActivityTypeId = activitytypes[1].Id,   StartDate = modules[10].StartDate,              EndDate = modules[10].StartDate.AddDays(1), DeadlineDate = modules[10].StartDate.AddDays(1) },
                new Activity { ModuleId = modules[10].Id,   Name = "Övning (SQLServ/MVC#0)",    ActivityTypeId = activitytypes[2].Id,   StartDate = modules[10].StartDate.AddDays(2),   EndDate = modules[10].StartDate.AddDays(5), DeadlineDate = modules[10].StartDate.AddDays(5) },

                new Activity { ModuleId = modules[14].Id,   Name = "Teori   (SQLServ/IT#1)",    ActivityTypeId = activitytypes[0].Id,   StartDate = modules[14].StartDate,               EndDate = modules[14].StartDate.AddDays(1), DeadlineDate = modules[14].StartDate.AddDays(1) },
                new Activity { ModuleId = modules[14].Id,   Name = "Övning  (SQLServ/IT#1)",    ActivityTypeId = activitytypes[2].Id,   StartDate = modules[14].StartDate.AddDays(2),    EndDate = modules[14].StartDate.AddDays(5), DeadlineDate = modules[14].StartDate.AddDays(5) },

                new Activity { ModuleId = modules[22].Id,   Name = "Teori  (SQLServ/MVC#1)",    ActivityTypeId = activitytypes[1].Id,   StartDate = modules[22].StartDate,              EndDate = modules[22].StartDate.AddDays(1), DeadlineDate = modules[22].StartDate.AddDays(1) },
                new Activity { ModuleId = modules[22].Id,   Name = "Övning (SQLServ/MVC#1)",    ActivityTypeId = activitytypes[2].Id,   StartDate = modules[22].StartDate.AddDays(2),   EndDate = modules[22].StartDate.AddDays(5), DeadlineDate = modules[22].StartDate.AddDays(5) },

                new Activity { ModuleId = modules[26].Id,   Name = "Teori  (SQLServ/MVC#2)",    ActivityTypeId = activitytypes[1].Id,   StartDate = modules[26].StartDate,              EndDate = modules[26].StartDate.AddDays(1), DeadlineDate = modules[26].StartDate.AddDays(1) },
                new Activity { ModuleId = modules[26].Id,   Name = "Övning (SQLServ/MVC#2)",    ActivityTypeId = activitytypes[2].Id,   StartDate = modules[26].StartDate.AddDays(2),   EndDate = modules[26].StartDate.AddDays(5), DeadlineDate = modules[26].StartDate.AddDays(5) },

            // Office 365 modulens aktiviteter (modules[3], [15])
                new Activity { ModuleId = modules[3].Id,    Name = "Teori (Office/IT#0)",   ActivityTypeId = activitytypes[0].Id,   StartDate = modules[3].StartDate,              EndDate = modules[3].StartDate.AddDays(1), DeadlineDate = modules[3].StartDate.AddDays(1) },
                new Activity { ModuleId = modules[3].Id,    Name = "Övning (Office/IT#0)",  ActivityTypeId = activitytypes[2].Id,   StartDate = modules[3].StartDate.AddDays(2),   EndDate = modules[3].StartDate.AddDays(5), DeadlineDate = modules[3].StartDate.AddDays(5) },

                new Activity { ModuleId = modules[15].Id,   Name = "Teori (Office/IT#1)",   ActivityTypeId = activitytypes[0].Id,   StartDate = modules[15].StartDate,             EndDate = modules[15].StartDate.AddDays(1), DeadlineDate = modules[15].StartDate.AddDays(1) },
                new Activity { ModuleId = modules[15].Id,   Name = "Övning (Office/IT#1)",  ActivityTypeId = activitytypes[2].Id,   StartDate = modules[15].StartDate.AddDays(2),  EndDate = modules[15].StartDate.AddDays(5), DeadlineDate = modules[15].StartDate.AddDays(5) },

            // OO-modulens aktiviteter i courses[1], [2], [4], [5], [6] (modules[5], [9], [17], [21], [25])
                new Activity { ModuleId = modules[5].Id,    Name = "Teori   (OO/Java#0)",   ActivityTypeId = activitytypes[1].Id,   StartDate = modules[5].StartDate,               EndDate = modules[5].StartDate.AddDays(1),  DeadlineDate = modules[5].StartDate.AddDays(1) },
                new Activity { ModuleId = modules[5].Id,    Name = "Övning  (OO/Java#0)",   ActivityTypeId = activitytypes[2].Id,   StartDate = modules[5].StartDate.AddDays(2),    EndDate = modules[5].StartDate.AddDays(5),  DeadlineDate = modules[5].StartDate.AddDays(5) },

                new Activity { ModuleId = modules[9].Id,    Name = "Teori   (OO/MVC#0)",    ActivityTypeId = activitytypes[0].Id,   StartDate = modules[9].StartDate,               EndDate = modules[9].StartDate.AddDays(1),  DeadlineDate = modules[9].StartDate.AddDays(1) },
                new Activity { ModuleId = modules[9].Id,    Name = "Övning  (OO/MVC#0)",    ActivityTypeId = activitytypes[2].Id,   StartDate = modules[9].StartDate.AddDays(2),    EndDate = modules[9].StartDate.AddDays(5),  DeadlineDate = modules[9].StartDate.AddDays(5) },

                new Activity { ModuleId = modules[17].Id,   Name = "Teori   (OO/Java#1)",   ActivityTypeId = activitytypes[1].Id,   StartDate = modules[17].StartDate,              EndDate = modules[17].StartDate.AddDays(1), DeadlineDate = modules[17].StartDate.AddDays(1) },
                new Activity { ModuleId = modules[17].Id,   Name = "Övning  (OO/Java#1)",   ActivityTypeId = activitytypes[2].Id,   StartDate = modules[17].StartDate.AddDays(2),   EndDate = modules[17].StartDate.AddDays(5), DeadlineDate = modules[17].StartDate.AddDays(5) },

                new Activity { ModuleId = modules[21].Id,   Name = "Teori   (OO/MVC#1)",    ActivityTypeId = activitytypes[0].Id,   StartDate = modules[21].StartDate,              EndDate = modules[21].StartDate.AddDays(1), DeadlineDate = modules[21].StartDate.AddDays(1) },
                new Activity { ModuleId = modules[21].Id,   Name = "Övning  (OO/MVC#1)",    ActivityTypeId = activitytypes[2].Id,   StartDate = modules[21].StartDate.AddDays(2),   EndDate = modules[21].StartDate.AddDays(5), DeadlineDate = modules[21].StartDate.AddDays(5) },

                new Activity { ModuleId = modules[25].Id,   Name = "Teori   (OO/MVC#2)",    ActivityTypeId = activitytypes[1].Id,   StartDate = modules[25].StartDate,              EndDate = modules[25].StartDate.AddDays(1), DeadlineDate = modules[25].StartDate.AddDays(1) },
                new Activity { ModuleId = modules[25].Id,   Name = "Övning  (OO/MVC#2)",    ActivityTypeId = activitytypes[2].Id,   StartDate = modules[25].StartDate.AddDays(2),   EndDate = modules[25].StartDate.AddDays(5), DeadlineDate = modules[25].StartDate.AddDays(5) },

            // Oracle-modulens aktiviteter (modules[6], [18])
                new Activity { ModuleId = modules[6].Id,    Name = "Teori   (Oracle/Java#0)",   ActivityTypeId = activitytypes[0].Id,   StartDate = modules[6].StartDate,                EndDate = modules[6].StartDate.AddDays(1),  DeadlineDate = modules[6].StartDate.AddDays(1) },
                new Activity { ModuleId = modules[6].Id,    Name = "Övning  (Oracle/Java#0)",   ActivityTypeId = activitytypes[2].Id,   StartDate = modules[6].StartDate.AddDays(2),     EndDate = modules[6].StartDate.AddDays(5),  DeadlineDate = modules[6].StartDate.AddDays(5) },

                new Activity { ModuleId = modules[18].Id,   Name = "Teori   (Oracle/Java#1)",   ActivityTypeId = activitytypes[0].Id,   StartDate = modules[18].StartDate,               EndDate = modules[18].StartDate.AddDays(1),  DeadlineDate = modules[18].StartDate.AddDays(1) },
                new Activity { ModuleId = modules[18].Id,   Name = "Övning  (Oracle/Java#1)",   ActivityTypeId = activitytypes[2].Id,   StartDate = modules[18].StartDate.AddDays(2),    EndDate = modules[18].StartDate.AddDays(5),  DeadlineDate = modules[18].StartDate.AddDays(5) },

            // Java-modulens aktiviteter (modules[7], [19])
                new Activity { ModuleId = modules[7].Id,    Name = "Teori   (Java#0)",  ActivityTypeId = activitytypes[1].Id,   StartDate = modules[7].StartDate,               EndDate = modules[7].StartDate.AddDays(1),  DeadlineDate = modules[7].StartDate.AddDays(1) },
                new Activity { ModuleId = modules[7].Id,    Name = "Övning  (Java#0)",  ActivityTypeId = activitytypes[2].Id,   StartDate = modules[7].StartDate.AddDays(2),    EndDate = modules[7].StartDate.AddDays(5),  DeadlineDate = modules[7].StartDate.AddDays(5) },

                new Activity { ModuleId = modules[19].Id,   Name = "Teori   (Java#1)",  ActivityTypeId = activitytypes[1].Id,   StartDate = modules[19].StartDate,               EndDate = modules[19].StartDate.AddDays(1),  DeadlineDate = modules[19].StartDate.AddDays(1) },
                new Activity { ModuleId = modules[19].Id,   Name = "Övning  (Java#1)",  ActivityTypeId = activitytypes[2].Id,   StartDate = modules[19].StartDate.AddDays(2),    EndDate = modules[19].StartDate.AddDays(5),  DeadlineDate = modules[19].StartDate.AddDays(5) },

            // MVC-modulens aktiviteter i courses [2], [5], [6] (modules[11], [23], [27])
                new Activity { ModuleId = modules[11].Id,   Name = "Teori  (MVC#0)",    ActivityTypeId = activitytypes[0].Id,   StartDate = modules[11].StartDate,              EndDate = modules[11].StartDate.AddDays(1), DeadlineDate = modules[11].StartDate.AddDays(1) },
                new Activity { ModuleId = modules[11].Id,   Name = "Övning (MVC#0)",    ActivityTypeId = activitytypes[2].Id,   StartDate = modules[11].StartDate.AddDays(2),   EndDate = modules[11].StartDate.AddDays(5), DeadlineDate = modules[11].StartDate.AddDays(5) },

                new Activity { ModuleId = modules[23].Id,   Name = "Teori  (MVC#1)",    ActivityTypeId = activitytypes[0].Id,   StartDate = modules[23].StartDate,              EndDate = modules[23].StartDate.AddDays(1), DeadlineDate = modules[23].StartDate.AddDays(1) },
                new Activity { ModuleId = modules[23].Id,   Name = "Övning (MVC#1)",    ActivityTypeId = activitytypes[2].Id,   StartDate = modules[23].StartDate.AddDays(2),   EndDate = modules[23].StartDate.AddDays(5), DeadlineDate = modules[23].StartDate.AddDays(5) },

                new Activity { ModuleId = modules[27].Id,   Name = "Teori  (MVC#2)",    ActivityTypeId = activitytypes[1].Id,   StartDate = modules[27].StartDate,              EndDate = modules[27].StartDate.AddDays(1), DeadlineDate = modules[27].StartDate.AddDays(1) },
                new Activity { ModuleId = modules[27].Id,   Name = "Övning (MVC#2)",    ActivityTypeId = activitytypes[2].Id,   StartDate = modules[27].StartDate.AddDays(2),   EndDate = modules[27].StartDate.AddDays(5), DeadlineDate = modules[27].StartDate.AddDays(5) }
            };

            context.Activities.AddOrUpdate(
                unik => unik.Name,
                activities
                );

            context.SaveChanges();
        }
    }
}
