using System.Linq;
using System.Web.Mvc;
using TimeManage.Models;
using Microsoft.AspNet.Identity;

namespace TimeManage.Controllers
{
    [Authorize]
    public class TasksController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        public ActionResult Index()
        {
            string userId = User.Identity.GetUserId();
            var todos = db.Tasks.Where(t => t.UserId == userId).ToList();
            return View(todos);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Tasks task)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage).ToList();

                System.Diagnostics.Debug.WriteLine("ModelState Errors: " + string.Join(", ", errors));
            }
            else
            {
                task.UserId = User.Identity.GetUserId();
                db.Tasks.Add(task);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(task);
        }

        [HttpPost]
        [Authorize]
        public ActionResult ToggleComplete(int id)
        {
            var todo = db.Tasks.Find(id);
            if (todo == null || todo.UserId != User.Identity.GetUserId())
                return HttpNotFound();

            todo.IsComplete = !todo.IsComplete;
            db.SaveChanges();

            return RedirectToAction("Index");
        }

        [HttpPost]
        [Authorize]
        public ActionResult Delete(int id)
        {
            var todo = db.Tasks.Find(id);
            if (todo == null || todo.UserId != User.Identity.GetUserId())
                return HttpNotFound();

            db.Tasks.Remove(todo);
            db.SaveChanges();

            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteAll()
        {
            var userId = User.Identity.GetUserId();
            var allTasks = db.Tasks.Where(t => t.UserId == userId);
            db.Tasks.RemoveRange(allTasks);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

    }
}
