using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using HEJARITO.Models;

namespace HEJARITO.Controllers
{
    [Authorize] //TM 2018-03-12 13:39
    public class ModulesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Modules
        public ActionResult Index()
        {
            var modules = db.Modules.Include(m => m.Course);
            return View(modules.ToList());
        }

        // JSON: return Activities in module
        public ActionResult GetActivities(int module)
        {
            db.Configuration.ProxyCreationEnabled = false;
            var activities = db.Activities.Where(c => c.ModuleId == module).ToList();
            var tmp = Json(activities, JsonRequestBehavior.AllowGet);
            return tmp;
        }
        public ActionResult GetDates(int moduleId)
        {
            Module m = db.Modules.Find(moduleId);
            string start = m.StartDate.Year.ToString("0000") + "-" + m.StartDate.Month.ToString("00") + "-" + m.StartDate.Day.ToString("00") + "T" + m.StartDate.Hour.ToString("00") + ":" + m.StartDate.Minute.ToString("00") + ":" + m.StartDate.Second.ToString("00");
            string end = m.EndDate.Year.ToString("0000") + "-" + m.EndDate.Month.ToString("00") + "-" + m.EndDate.Day.ToString("00") + "T" + m.EndDate.Hour.ToString("00") + ":" + m.EndDate.Minute.ToString("00") + ":" + m.EndDate.Second.ToString("00");

            return Json(new string[] { start, end }, JsonRequestBehavior.AllowGet);
        }
        // GET: Modules/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Module module = db.Modules.Find(id);
            if (module == null)
            {
                return HttpNotFound();
            }
            return View(module);
        }

        // GET: Modules/Create
        [Authorize(Roles = "Teacher")] //TM 2018-03-12 13:39
        public ActionResult Create()
        {
            ViewBag.CourseId = new SelectList(db.Courses, "Id", "Name");
            return View();
        }

        // POST: Modules/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "Teacher")] //TM 2018-03-12 13:39
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,Description,StartDate,EndDate,CourseId")] Module module)
        {
            if (ModelState.IsValid)
            {
                module.EndDate = module.EndDate.Add(new TimeSpan(23, 59, 59));
                db.Modules.Add(module);
                db.SaveChanges();

                //TM 2018-03-19 16-19 Ska visas i nästa vy
                ViewBag.KvittoMeddelande = "Skapande av en ny modul genomfördes";

                return RedirectToAction("Index");
            }

            ViewBag.CourseId = new SelectList(db.Courses, "Id", "Name", module.CourseId);
            return View(module);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public PartialViewResult CreateAjax([Bind(Include = "Id,Name,Description,StartDate,EndDate,CourseId")] Module module)
        {
            Course course = db.Courses.Find(module.CourseId);

            List<Module> lm = course.Modules.OrderBy(m => m.StartDate).ToList();
            if (course != null && ModelState.IsValid)
            {
                module.Activities = new List<Activity>();
                module.Documents = new List<Document>();
                module.EndDate = module.EndDate.Add(new TimeSpan(23, 59, 59));
                db.Modules.Add(module);
                db.SaveChanges();

                //TM 2018-03-19 16-19 Ska visas i nästa vy
                ViewBag.KvittoMeddelande = "Skapande av en ny modul genomfördes";

                return PartialView("_CourseModulesEditor", lm);
            }
            else
            {
                List<ModelError> l = new List<ModelError>();
                foreach (var item in ModelState.Values)
                {
                    foreach (var error in item.Errors)
                    {
                        if (!l.Any(e => e.ErrorMessage == error.ErrorMessage))
                        {
                            l.Add(error);
                        }
                        
                    }
                    
                }
                ViewBag.errorMessages = l ;
            }

            return PartialView("_CourseModulesEditor", lm);
        }

        // GET: Modules/Edit/5
        [Authorize(Roles = "Teacher")] //TM 2018-03-12 13:39
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Module module = db.Modules.Find(id);
            if (module == null)
            {
                return HttpNotFound();
            }
            ViewBag.CourseId = new SelectList(db.Courses, "Id", "Name", module.CourseId);
            return View(module);
        }

        // POST: Modules/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "Teacher")] //TM 2018-03-12 13:39
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,Description,StartDate,EndDate,CourseId")] Module module)
        {
            if (ModelState.IsValid)
            {
                db.Entry(module).State = EntityState.Modified;
                db.SaveChanges();

                //TM 2018-03-19 16-19 Ska visas i nästa vy
                ViewBag.KvittoMeddelande = "Redigering av en modul genomfördes";

                //return RedirectToAction("Index");
                int Id = Int32.Parse(Request["Id"]);
                return RedirectToAction("Details", "Modules", new { id = Id });
            }
            ViewBag.CourseId = new SelectList(db.Courses, "Id", "Name", module.CourseId);
            return View(module);
        }

        // GET: Modules/Delete/5
        [Authorize(Roles = "Teacher")] //TM 2018-03-12 13:39
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Module module = db.Modules.Find(id);
            if (module == null)
            {
                return HttpNotFound();
            }
            return View(module);
        }

        // POST: Modules/Delete/5
        [Authorize(Roles = "Teacher")] //TM 2018-03-12 13:39
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Module module = db.Modules.Find(id);
            db.Modules.Remove(module);
            db.SaveChanges();

            //TM 2018-03-19 16-19 Ska visas i nästa vy
            ViewBag.KvittoMeddelande = "Borttagning av en modul genomfördes";

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