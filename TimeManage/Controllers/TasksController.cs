using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using TimeManage.Models;

namespace TimeManage.Controllers
{
    public class TasksController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Tasks
        public ActionResult Index()
        {
            var userId = User.Identity.GetUserId();
            var tasks = db.Tasks.Where(t => t.UserId == userId).Include(t => t.User);
            return View(tasks.ToList());
        }

        // GET: Tasks/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var userId = User.Identity.GetUserId();
            Tasks tasks = db.Tasks.FirstOrDefault(t => t.Id == id && t.UserId == userId);

            if (tasks == null)
            {
                return HttpNotFound();
            }
            return View(tasks);
        }

        // GET: Tasks/Create
        public ActionResult Create()
        {
            ViewBag.UserId = new SelectList(db.Users, "Id", "DisplayName");
            return View();
        }

        // POST: Tasks/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Title,Description,IsComplete")] Tasks tasks)
        {
            if (ModelState.IsValid)
            {
                tasks.UserId = User.Identity.GetUserId();
                db.Tasks.Add(tasks);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.UserId = new SelectList(db.Users, "Id", "DisplayName", tasks.UserId);
            return View(tasks);
        }

        // GET: Tasks/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var userId = User.Identity.GetUserId();
            Tasks tasks = db.Tasks.FirstOrDefault(t => t.Id == id && t.UserId == userId);

            if (tasks == null)
            {
                return HttpNotFound();
            }

            ViewBag.UserId = new SelectList(db.Users, "Id", "DisplayName", tasks.UserId);
            return View(tasks);
        }

        // POST: Tasks/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Title,Description,IsComplete,UserId")] Tasks tasks)
        {
            var userId = User.Identity.GetUserId();

            // Ensure the task belongs to the current user
            if (tasks.UserId != userId)
            {
                return new HttpStatusCodeResult(HttpStatusCode.Forbidden);
            }

            if (ModelState.IsValid)
            {
                db.Entry(tasks).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.UserId = new SelectList(db.Users, "Id", "DisplayName", tasks.UserId);
            return View(tasks);
        }

        

        // GET: Tasks/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var userId = User.Identity.GetUserId();
            Tasks tasks = db.Tasks.FirstOrDefault(t => t.Id == id && t.UserId == userId);

            if (tasks == null)
            {
                return HttpNotFound();
            }
            return View(tasks);
        }

        // POST: Tasks/Delete/5
        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            var userId = User.Identity.GetUserId();
            Tasks tasks = db.Tasks.FirstOrDefault(t => t.Id == id && t.UserId == userId);

            if (tasks != null)
            {
                db.Tasks.Remove(tasks);
                db.SaveChanges();
            }

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


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteAll()
        {
            string userId = User.Identity.GetUserId();
            var userTasks = db.Tasks.Where(t => t.UserId == userId).ToList();
            db.Tasks.RemoveRange(userTasks);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

    }
}
