using System.Linq;
using System.Web.Mvc;
using TimeManage.Models;
using Microsoft.AspNet.Identity;

namespace TimeManage.Controllers
{
    [Authorize]
    public class ToDoController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        public ActionResult Index()
        {
            string userId = User.Identity.GetUserId();
            var todos = db.ToDos.Where(t => t.UserId == userId).ToList();
            return View(todos);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ToDo todo)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage).ToList();

                System.Diagnostics.Debug.WriteLine("ModelState Errors: " + string.Join(", ", errors));
            }
            else
            {
                todo.UserId = User.Identity.GetUserId();
                db.ToDos.Add(todo);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(todo);
        }

        [HttpPost]
        [Authorize]
        public ActionResult ToggleComplete(int id)
        {
            var todo = db.ToDos.Find(id);
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
            var todo = db.ToDos.Find(id);
            if (todo == null || todo.UserId != User.Identity.GetUserId())
                return HttpNotFound();

            db.ToDos.Remove(todo);
            db.SaveChanges();

            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteAll()
        {
            var userId = User.Identity.GetUserId();
            var allTasks = db.ToDos.Where(t => t.UserId == userId);
            db.ToDos.RemoveRange(allTasks);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

    }
}
