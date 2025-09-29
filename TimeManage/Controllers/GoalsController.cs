using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using TimeManage.Models;
using TimeManage.ViewModels;
using Microsoft.AspNet.Identity; // Add this

namespace TimeManage.Controllers
{
    public class GoalsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Goals
        public ActionResult Index()
        {
            var userId = User.Identity.GetUserId();
            var goals = db.Goals.Where(g => g.UserId == userId).ToList();
            return View(goals);
        }

        // GET: Goals/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var userId = User.Identity.GetUserId();
            Goal goal = db.Goals.FirstOrDefault(g => g.GoalId == id && g.UserId == userId);

            if (goal == null)
            {
                return HttpNotFound();
            }
            return View(goal);
        }

        // GET: Goals/Create
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "GoalName,GoalDescription")] Goal goal)
        {
            var userId = User?.Identity?.GetUserId();
            if (string.IsNullOrEmpty(userId))
            {
                return new HttpStatusCodeResult(HttpStatusCode.Unauthorized);
            }

            goal.UserId = userId;
            ModelState.Remove("UserId");

            if (ModelState.IsValid)
            {
                db.Goals.Add(goal);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(goal);
        }



        // GET: Goals/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var userId = User.Identity.GetUserId();
            Goal goal = db.Goals.FirstOrDefault(g => g.GoalId == id && g.UserId == userId);

            if (goal == null)
            {
                return HttpNotFound();
            }
            return View(goal);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "GoalId,GoalName,GoalDescription")] Goal goal)
        {
            var userId = User.Identity.GetUserId();

            var existingGoal = db.Goals.Find(goal.GoalId);
            if (existingGoal == null || existingGoal.UserId != userId)
            {
                return new HttpStatusCodeResult(HttpStatusCode.Forbidden);
            }

            ModelState.Remove("UserId");

            if (ModelState.IsValid)
            {
                existingGoal.GoalName = goal.GoalName;
                existingGoal.GoalDescription = goal.GoalDescription;

                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(goal);
        }




        // GET: Goals/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var userId = User.Identity.GetUserId();
            Goal goal = db.Goals.FirstOrDefault(g => g.GoalId == id && g.UserId == userId);

            if (goal == null)
            {
                return HttpNotFound();
            }
            return View(goal);
        }

        // POST: Goals/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            var userId = User.Identity.GetUserId();
            Goal goal = db.Goals.FirstOrDefault(g => g.GoalId == id && g.UserId == userId);

            if (goal != null)
            {
                goal.Tasks.Clear();
                db.Goals.Remove(goal);
                db.SaveChanges();
            }

            return RedirectToAction("Index");
        }

        public ActionResult AddTasks(int? id)
        {
            var userId = User.Identity.GetUserId();
            var userTasks = db.Tasks.Where(t => t.UserId == userId).ToList(); // Only user's tasks
            Goal goal = db.Goals.FirstOrDefault(g => g.GoalId == id && g.UserId == userId);

            if (goal == null)
            {
                return HttpNotFound();
            }

            GoalTaskViewModel model = new GoalTaskViewModel { goal = goal, Tasks = userTasks };
            return View(model);
        }

        public ActionResult AddTaskToGoal(int? GoalId, int? taskId)
        {
            var userId = User.Identity.GetUserId();
            var goal = db.Goals.FirstOrDefault(g => g.GoalId == GoalId && g.UserId == userId);
            var task = db.Tasks.FirstOrDefault(t => t.Id == taskId && t.UserId == userId);

            if (goal != null && task != null)
            {
                goal.Tasks.Add(task);
                db.SaveChanges();
            }
            else
            {
                return HttpNotFound();
            }

            return RedirectToAction("Details", new { id = goal.GoalId });
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