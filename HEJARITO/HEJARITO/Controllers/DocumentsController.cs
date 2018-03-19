using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
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
            var documents = db.Documents.Include(d => d.Activity).Include(d => d.Course).Include(d => d.Module);
            return View(documents.ToList());
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
                    break;
                case DocumentType.ModuleDocument:
                    document.Module = db.Modules.Find(document.ModuleId);
                    localPath += "/Course_" + document.Module.CourseId + "/Module_" + document.ModuleId;
                    break;
                case DocumentType.ActivityDocument:
                    document.Activity = db.Activities.Find(document.ActivityId);
                    localPath += "/Course_" + document.Activity.Module.CourseId + "/Module_" + document.Activity.ModuleId + "/Activity_" + document.ActivityId;
                    break;
                default:
                    break;
            }
            string _path = Path.Combine(Server.MapPath(localPath), _FileName);

            return new FilePathResult(_path, document.ContentType);
        }



        // GET: Documents/Create
        public ActionResult Create()
        {
            //ViewBag.ActivityId = new SelectList(db.Activities, "Id", "Name");
            //ViewBag.CourseId = new SelectList(db.Courses, "Id", "Name");
            //ViewBag.ModuleId = new SelectList(db.Modules, "Id", "Name");

            ViewBag.ActivityTypes   = db.ActivityTypes.ToList();
            ViewBag.Courses         = db.Courses.ToList();
            ViewBag.Modules         = db.Modules.ToList();
            ViewBag.Activities      = db.Activities.ToList();
            return View();
        }

        // POST: Documents/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,Description,DocumentType,CourseId,ModuleId,ActivityId,FileName")] Document document, HttpPostedFileBase file)
        {
            document.UploadDate = DateTime.Now;
            document.ApplicationUserId = User.Identity.GetUserId();
            if (ModelState.IsValid)
            {
                try
                {
                    if (file.ContentLength > 0)
                    {
                        string _FileName = Path.GetFileName(file.FileName);
                        string localPath = "~/UploadedFiles";
                        switch (document.DocumentType)
                        {
                            case DocumentType.CourseDocument:
                                localPath += "/Course_" + document.CourseId;
                                break;
                            case DocumentType.ModuleDocument:
                                document.Module = db.Modules.Find(document.ModuleId);
                                localPath += "/Course_" + document.Module.CourseId + "/Module_" + document.ModuleId;
                                break;
                            case DocumentType.ActivityDocument:
                                document.Activity = db.Activities.Find(document.ActivityId);
                                localPath += "/Course_" + document.Activity.Module.CourseId + "/Module_" + document.Activity.ModuleId + "/Activity_" + document.ActivityId;
                                break;
                            default:
                                break;
                        }
                        if (!Directory.Exists(Server.MapPath(localPath)))
                            Directory.CreateDirectory(Server.MapPath(localPath));
                        string _path = Path.Combine(Server.MapPath(localPath), _FileName);
                        document.ContentLength = file.ContentLength;
                        document.FileName = file.FileName;
                        document.ContentType = file.ContentType;
                        document.ApplicationUser = db.Users.Find(document.ApplicationUserId);
                        file.SaveAs(_path);
                    }
                    db.Documents.Add(document);
                    db.SaveChanges();
                    ViewBag.Message = "Filen: " + file.FileName + " är nu uppladdad!!";
                }
                catch
                {
                    ViewBag.Message = "Kunde inte ladda upp filen!!";
                }
                return RedirectToAction("Index");
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
        public ActionResult DeleteConfirmed(int id)
        {
            Document document = db.Documents.Find(id);
            db.Documents.Remove(document);
            db.SaveChanges();
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
