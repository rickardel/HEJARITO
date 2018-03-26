using HEJARITO.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HEJARITO.Controllers
{
    public class HomeController : Controller
    {
        //TM 2018-03-09 15:09 Genom raden nedan får vi tillgång till vår databas
        private ApplicationDbContext db = new ApplicationDbContext();

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
            ViewBag.Message = "Vår kontaktsida";

            return View();
        }

        public ActionResult SelectStartView()
        {
            if (User.IsInRole("Teacher"))
            {
                return RedirectToAction("Teacher");
            }
            if (User.IsInRole("Student"))
            {
                return RedirectToAction("Student");
            }
            else
            {
                //TM 2018-03-26 11:05 Icke-inloggad användare ska hamna på välkomstsidan enligt Rickard
                return RedirectToAction("Index");
            }
        }

        [Authorize(Roles = "Teacher")] //TM 2018-03-12 13:38
        //JG 2018-03-12 Sidan som en nyss inloggad lärare hamnar på
        public ActionResult Teacher()
        {
            ViewBag.Message = "Teacher's start page.";

            TeacherViewModel teacherViewModel = new TeacherViewModel();

            // Calculating the start and end dates of the current week
            DateTime today = DateTime.Now;
            int start = 0;
            int end = 0;
            if (today.DayOfWeek == DayOfWeek.Monday)
            {
                start = 0; end = 6;
            }
            if (today.DayOfWeek == DayOfWeek.Tuesday)
            {
                start = -1; end = 5;
            }
            if (today.DayOfWeek == DayOfWeek.Wednesday)
            {
                start = -2; end = 4;
            }
            if (today.DayOfWeek == DayOfWeek.Thursday)
            {
                start = -3; end = 3;
            }
            if (today.DayOfWeek == DayOfWeek.Friday)
            {
                start = -4; end = 2;
            }
            if (today.DayOfWeek == DayOfWeek.Saturday)
            {
                start = -5; end = 1;
            }
            if (today.DayOfWeek == DayOfWeek.Sunday)
            {
                start = -6; end = 0;
            }
            TimeSpan startOffset = new TimeSpan(start, 0, 0, 0);
            TimeSpan endOffset = new TimeSpan(end, 0, 0, 0);
            DateTime startDate = DateTime.Now.Add(startOffset).Date;
            DateTime endDate = DateTime.Now.Add(endOffset).Date;

            // Intermediate decision: show starting and ending activities, and deadlines for the current week
            List<Activity> startingActivities = new List<Activity>();
            List<Activity> endingActivities = new List<Activity>();
            List<Activity> deadLines = new List<Activity>();
            //startingActivities = db.Activities.Where(d => d.StartDate >= startDate && d.StartDate <= endDate).ToList();
            //endingActivities = db.Activities.Where(d => d.EndDate >= startDate && d.EndDate <= endDate).ToList();
            //deadLines = db.Activities.Where(d => d.DeadlineDate >= startDate && d.DeadlineDate <= endDate).ToList();
            //teacherViewModel.Activities = startingActivities.Concat(endingActivities).Concat(deadLines).ToList();
            teacherViewModel.Activities = db.Activities.Where(d => d.EndDate >= startDate && d.EndDate <= endDate).ToList();
            if (teacherViewModel.Activities.Count > 3)
            {
                ViewBag.TableSizeActivities = "large";
            }
            else
            {
                ViewBag.TableSizeActivities = "small";
            }

            teacherViewModel.Courses = db.Courses.ToList();
            if (teacherViewModel.Courses.Count > 3)
            {
                ViewBag.TableSizeCourses = "large";
            }
            else
            {
                ViewBag.TableSizeCourses = "small";
            }

            teacherViewModel.Users = db.Users.ToList();
            if (teacherViewModel.Users.Count > 3)
            {
                ViewBag.TableSizeContacts = "large";
            }
            else
            {
                ViewBag.TableSizeContacts = "small";
            }

            return View(teacherViewModel);
        }

        [Authorize(Roles = "Student")] //TM 2018-03-12 13:38
        public ActionResult Student()
        //TM 2018-03-09 23:24 Sidan som en nyss inloggad elev hamnar i
        {
            ViewBag.Message = "Student's start page.";

            var studentViewModel = new StudentViewModel();

            //TM 2018-03-10 02:16 Utan dessa 2 rader kraschar .Add(myCourse) !!!
            studentViewModel.Courses = db.Courses.ToList(); //!!! #1
            studentViewModel.Courses.Clear();               //!!! #2

            var myCourseId = db.Users.Find(User.Identity.GetUserId()).CourseId;

            if (myCourseId != null)
            {
                var myCourse = db.Courses.Find(myCourseId);

                if (myCourse != null)
                {
                    studentViewModel.Courses.Add(myCourse); //!!! #3
                }
            }

            //TM 2018-03-13 15:44 Genom denna loop skapas listan av de moduler som ingår i elevens kurs!
            //TM 2018-03-13 15:53 Utan dessa 2 rader kraschar .Add(mm) !!!
            studentViewModel.Modules = db.Modules.ToList(); //!!! #1
            studentViewModel.Modules.Clear();               //!!! #2

            var myModuleList = db.Modules.ToList();

            foreach (var mm in myModuleList)
            {
                if (mm.CourseId == myCourseId)
                {
                    studentViewModel.Modules.Add(mm);       //!!! #3
                }
            }

            //TM 2018-03-13 23:05 Räkna fram aktuell veckas start- och slutdatum

            DateTime today = DateTime.Now;
            int start;
            int end = 0;

            switch (today.DayOfWeek)
            {
                case DayOfWeek.Monday:
                    end = 6;
                    break;
                case DayOfWeek.Tuesday:
                    end = 5;
                    break;
                case DayOfWeek.Wednesday:
                    end = 4;
                    break;
                case DayOfWeek.Thursday:
                    end = 3;
                    break;
                case DayOfWeek.Friday:
                    end = 2;
                    break;
                case DayOfWeek.Saturday:
                    end = 1;
                    break;
                case DayOfWeek.Sunday:
                    end = 0;
                    break;
                default:
                    break;
            }

            start = end - 6;

            TimeSpan startOffset = new TimeSpan(start, 0, 0, 0);
            TimeSpan endOffset = new TimeSpan(end, 0, 0, 0);
            DateTime thisMonday = DateTime.Now.Add(startOffset).Date;
            DateTime thisSunday = DateTime.Now.Add(endOffset).Date;

            //TM 2018-03-13 15:53 Utan dessa 2 rader kraschar .Add(a) !!!
            studentViewModel.Activities = db.Activities.ToList();   //!!! #1
            studentViewModel.Activities.Clear();                    //!!! #2

            var myActivityList = db.Activities.ToList();

            //TM 2018-03-13 23:18 Genom denna dubbelloop skapas listan av de veckoaktuella aktiviteter som ingår i någon modul som i sin tur ingår i elevens kurs!
            foreach (var m in studentViewModel.Modules)
            {
                if (m.CourseId == myCourseId)
                {
                    foreach (var a in myActivityList)
                    {
                        if (a.ModuleId == m.Id)
                        {
                            //TM 2018-03-13 23:16 Aktuell vecka berörs av aktiviteten
                            if (a.StartDate <= thisSunday && ((a.EndDate >= thisMonday) || (a.DeadlineDate >= thisMonday)))
                            {
                                studentViewModel.Activities.Add(a); //!!! #3
                            }
                        }
                    }
                }
            }

            //TM 2018-03-13 15:49 Genom denna loop skapas listan av elevens kurskamrater!
            //TM 2018-03-13 15:56 Utan dessa 2 rader kraschar .Add(u) !!!
            studentViewModel.Users = db.Users.ToList(); //!!! #1
            studentViewModel.Users.Clear();             //!!! #2

            var myUserList = db.Users.ToList();

            foreach (var u in myUserList)
            {
                if (u.CourseId != null)
                {
                    if (u.CourseId == myCourseId && (u.Id != User.Identity.GetUserId()))
                    {
                        studentViewModel.Users.Add(u);  //!!! #3
                    }
                }
            }

            return View(studentViewModel);
        }
    }
}