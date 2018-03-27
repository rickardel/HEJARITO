using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using HEJARITO.Models;
using Microsoft.AspNet.Identity;

namespace HEJARITO.Controllers
{
    public class DocumentsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Documents
        public ActionResult Index()
        {
            ICollection<Document> documents = db.Documents.Include(d => d.Activity).Include(d => d.Course).Include(d => d.Module).ToList();
            return View(documents);
        }

        // GET: Documents/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Document document = db.Documents.Find(id);
            if (document == null)
            {
                return HttpNotFound();
            }
            return View(document);
        }

        // GET: Documents/Download
        public ActionResult Download(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Document document = db.Documents.Find(id);
            if (document == null)
            {
                return HttpNotFound();
            }
            string _FileName = Path.GetFileName(document.FileName);
            string localPath = "~/UploadedFiles";
            switch (document.DocumentType)
            {
                case DocumentType.CourseDocument:
                    localPath += "/Course_" + document.CourseId;
                    //document.ActivityId = null;
                    //document.ModuleId = null;
                    break;
                case DocumentType.ModuleDocument:
                    document.Module = db.Modules.Find(document.ModuleId);
                    localPath += "/Course_" + document.Module.CourseId + "/Module_" + document.ModuleId;
                    //document.CourseId = null;
                    //document.ActivityId = null;
                    break;
                case DocumentType.ActivityDocument:
                    document.Activity = db.Activities.Find(document.ActivityId);
                    localPath += "/Course_" + document.Activity.Module.CourseId + "/Module_" + document.Activity.ModuleId + "/Activity_" + document.ActivityId;
                    //document.ModuleId = null; document.CourseId = null;
                    break;
                default:
                    break;
            }
            string _path = Path.Combine(Server.MapPath(localPath), _FileName);

            return new FilePathResult(_path, document.ContentType);
        }


        // GET: Documents/Create
        public ActionResult PartialCreate(int courseId, int documentType, int? moduleId, int? activityId)
        {
            //ViewBag.ActivityId = new SelectList(db.Activities, "Id", "Name");
            //ViewBag.CourseId = new SelectList(db.Courses, "Id", "Name");
            //ViewBag.ModuleId = new SelectList(db.Modules, "Id", "Name");

            ViewBag.ActivityTypes = db.ActivityTypes.ToList();
            ViewBag.Courses = db.Courses.ToList();
            ViewBag.Modules = db.Modules.ToList();
            ViewBag.Activities = db.Activities.ToList();
            DocumentType dt = DocumentType.CourseDocument; ;
            Document c = new Document() { DocumentType = dt, CourseId = courseId, ApplicationUserId = User.Identity.GetUserId() };
            switch (documentType)
            {
                case 0:
                    dt = DocumentType.CourseDocument;
                    break;
                case 1:
                    dt = DocumentType.ModuleDocument;
                    c = new Document() { DocumentType = dt, CourseId = courseId, ApplicationUserId = User.Identity.GetUserId() };
                    break;
                case 2:
                    dt = DocumentType.ActivityDocument;
                    c = new Document() { DocumentType = dt, CourseId = courseId, ApplicationUserId = User.Identity.GetUserId() };
                    break;
            }
            dt = DocumentType.CourseDocument;
            return PartialView("_CreateDocument", c);
        }

        // GET: Documents/Create
        public ActionResult CreateModuleDocument(int moduleId)
        {
            Module m = db.Modules.Find(moduleId);
            ViewBag.ActivityTypes = db.ActivityTypes.ToList();
            return View(new Document() { CourseId = m.Course.Id, ModuleId = m.Id, DocumentType = DocumentType.ModuleDocument });
        }
        public ActionResult CreateActivityDocument(int activityId)
        {
            Activity a = db.Activities.Find(activityId);
            ViewBag.ActivityTypes = db.ActivityTypes.ToList();
            return View(new Document() { CourseId = a.Module.Course.Id, ModuleId = a.Module.Id, ActivityId = a.Id, DocumentType = DocumentType.ActivityDocument });
        }
        public ActionResult Create(int? courseId)
        {
            ViewBag.ActivityTypes = db.ActivityTypes.ToList();
            ViewBag.Courses = db.Courses.ToList();
            ViewBag.Modules = db.Modules.ToList();
            ViewBag.Activities = db.Activities.ToList();
            Document c = new Document() { CourseId = courseId };
            return View(c);
        }

        // POST: Documents/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,Description,DocumentType,CourseId,ModuleId,ActivityId,FileName")] Document document, HttpPostedFileBase fileName)
        {
            document.UploadDate = DateTime.Now;
            document.ApplicationUserId = User.Identity.GetUserId();
            if (ModelState.IsValid)
            {
                try
                {
                    if (fileName.ContentLength > 0)
                    {
                        string _FileName = Path.GetFileName(fileName.FileName);
                        string localPath = "~/UploadedFiles";
                        switch (document.DocumentType)
                        {
                            case DocumentType.CourseDocument:
                                localPath += "/Course_" + document.CourseId;
                                document.ModuleId = null; document.ActivityId = null;
                                break;
                            case DocumentType.ModuleDocument:
                                document.Module = db.Modules.Find(document.ModuleId);
                                localPath += "/Course_" + document.Module.CourseId + "/Module_" + document.ModuleId;
                                document.CourseId = null; document.ActivityId = null;
                                break;
                            case DocumentType.ActivityDocument:
                                document.Activity = db.Activities.Find(document.ActivityId);
                                localPath += "/Course_" + document.Activity.Module.CourseId + "/Module_" + document.Activity.ModuleId + "/Activity_" + document.ActivityId;
                                document.ModuleId = null; document.CourseId = null;
                                break;
                            default:
                                break;
                        }
                        if (!Directory.Exists(Server.MapPath(localPath)))
                            Directory.CreateDirectory(Server.MapPath(localPath));
                        string _path = Path.Combine(Server.MapPath(localPath), _FileName);
                        document.ContentLength = fileName.ContentLength;
                        document.FileName = fileName.FileName;
                        document.ContentType = fileName.ContentType;
                        document.ApplicationUser = db.Users.Find(document.ApplicationUserId);
                        fileName.SaveAs(_path);
                    }
                    db.Documents.Add(document);
                    db.SaveChanges();
                    ViewBag.successMessage = "Filen: " + fileName.FileName + " är nu uppladdad!!";
                }
                catch
                {
                    ViewBag.errorMessages = "Kunde inte ladda upp filen!!";
                }
                return Redirect(ControllerContext.HttpContext.Request.UrlReferrer.ToString());
            }

            ViewBag.ActivityId = new SelectList(db.Activities, "Id", "Name", document.ActivityId);
            ViewBag.CourseId = new SelectList(db.Courses, "Id", "Name", document.CourseId);
            ViewBag.ModuleId = new SelectList(db.Modules, "Id", "Name", document.ModuleId);
            return View(document);
        }
        // GET: Documents/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Document document = db.Documents.Find(id);
            if (document == null)
            {
                return HttpNotFound();
            }
            ViewBag.ActivityId = new SelectList(db.Activities, "Id", "Name", document.ActivityId);
            ViewBag.CourseId = new SelectList(db.Courses, "Id", "Name", document.CourseId);
            ViewBag.ModuleId = new SelectList(db.Modules, "Id", "Name", document.ModuleId);
            return View(document);
        }

        // POST: Documents/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,Description,UploadDate,DocumentType,CourseId,ModuleId,ActivityId,FileName,ContentLength,ContentType")] Document document)
        {
            if (ModelState.IsValid)
            {
                db.Entry(document).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ActivityId = new SelectList(db.Activities, "Id", "Name", document.ActivityId);
            ViewBag.CourseId = new SelectList(db.Courses, "Id", "Name", document.CourseId);
            ViewBag.ModuleId = new SelectList(db.Modules, "Id", "Name", document.ModuleId);
            return View(document);
        }

        // GET: Documents/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Document document = db.Documents.Find(id);
            if (document == null)
            {
                return HttpNotFound();
            }
            return View(document);
        }

        // POST: Documents/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id, string returnController, string returnAction, string returnId)
        {
            Document document = db.Documents.Find(id);
            db.Documents.Remove(document);
            db.SaveChanges();

            return RedirectToAction(returnAction, returnController, new { Id = returnId });
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
