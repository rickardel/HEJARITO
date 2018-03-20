using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using HEJARITO.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace HEJARITO.Controllers
{
    [Authorize] //TM 2018-03-12 13:38
    public class CoursesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Courses
        public ActionResult Index()
        {
            return View(db.Courses.ToList());
        }

        // JSON: return Modules in course
        public ActionResult GetModules(int course)
        {
            db.Configuration.ProxyCreationEnabled = false;
            var modules = db.Modules.Where(c => c.CourseId == course).ToList();
            var tmp = Json(modules, JsonRequestBehavior.AllowGet);
            return tmp;
        }

        // GET: Courses/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Course course = db.Courses.Find(id);
            ViewBag.newActivityActivityType = new SelectList(db.ActivityTypes, "Id", "Name");
            ViewBag.newActivityModule = new SelectList(course.Modules, "Id", "Name");
            ViewBag.CourseId = new SelectList(db.Courses, "Id", "Name");
            if (course == null)
            {
                return HttpNotFound();
            }
            return View(course);
        }

        // GET: Courses/Create
        [Authorize(Roles = "Teacher")] //TM 2018-03-12 13:38
        public ActionResult Create()
        {
            Course tmpCourse = new Course()
            {
                StartDate = DateTime.Now,
                EndDate = DateTime.Now
            };
            return View(tmpCourse);
        }

        // POST: Courses/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "Teacher")] //TM 2018-03-12 13:38
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,Description,StartDate,EndDate")] Course course)
        {
            if (ModelState.IsValid)
            {
                db.Courses.Add(course);
                db.SaveChanges();

                //TM 2018-03-19 16-19 Ska visas i nästa vy
                ViewBag.KvittoMeddelande = "Skapande av en ny kurs genomfördes";

                return RedirectToAction("Index");
            }

            return View(course);
        }

        public ActionResult CourseEditor(int? id)
        {
            
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Course course = db.Courses.Find(id);
            if (course == null)
            {
                return HttpNotFound();
            }
            ViewBag.newActivityActivityType = new SelectList(db.ActivityTypes, "Id", "Name");
            ViewBag.newActivityModule = new SelectList(course.Modules, "Id", "Name");

            CourseEditor courseEditor = new CourseEditor(course);
            ViewBag.ActivityTypeId =               new SelectList(db.ActivityTypes.ToList(), "Id", "Name");
            ViewBag.ModuleId =                          new SelectList(courseEditor.Course.Modules, "Id", "Name");
            ViewBag.ActivityTypes = db.ActivityTypes.ToList();
            ViewBag.Module = course.Modules.ToList();
            return View(courseEditor);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public PartialViewResult CreateStudentAjax([Bind(Include = "Id,CourseId,FirstName,LastName,Email,PhoneNumber")] ApplicationUser student)
        {
            Course course = db.Courses.Find(student.CourseId);
            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(db));
            var user = new Models.ApplicationUser()
            {
                CourseId = student.CourseId,
                FirstName = student.FirstName,
                LastName = student.LastName,
                Email = student.Email,
                UserName = student.Email,
                PhoneNumber = student.PhoneNumber,
            };
            var result = userManager.Create(user, "password");

            if (!result.Succeeded)
            {
                throw new Exception(string.Join("\n", result.Errors));
            }
            else
            {
                userManager.AddToRole(user.Id, Role.Student);
            }
            
            return PartialView("_CourseStudents", course.ApplicationUsers);

        }

        // GET: Courses/Edit/5
        [Authorize(Roles = "Teacher")] //TM 2018-03-12 13:38
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Course course = db.Courses.Find(id);
            if (course == null)
            {
                return HttpNotFound();
            }
            return View(course);
        }

        // POST: Courses/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "Teacher")] //TM 2018-03-12 13:38
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,Description,StartDate,EndDate")] Course course)
        {
            if (ModelState.IsValid)
            {
                db.Entry(course).State = EntityState.Modified;
                db.SaveChanges();

                //TM 2018-03-19 16-19 Ska visas i nästa vy
                ViewBag.KvittoMeddelande = "Redigering av en kurs genomfördes";

                return RedirectToAction("Index");
            }
            return View(course);
        }

        // GET: Courses/Delete/5
        [Authorize(Roles = "Teacher")] //TM 2018-03-12 13:38
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Course course = db.Courses.Find(id);
            if (course == null)
            {
                return HttpNotFound();
            }
            return View(course);
        }

        // POST: Courses/Delete/5
        [Authorize(Roles = "Teacher")] //TM 2018-03-12 13:38
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Course course = db.Courses.Find(id);
            db.Courses.Remove(course);
            db.SaveChanges();

            //TM 2018-03-19 16-19 Ska visas i nästa vy
            ViewBag.KvittoMeddelande = "Bortagning av en kurs genomfördes";

            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}