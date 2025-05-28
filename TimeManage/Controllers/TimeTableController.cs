using Microsoft.AspNet.Identity;
using System;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using TimeManage.Models;

namespace TimeManage.Controllers
{
    [Authorize]
    public class TimeTableController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        public ActionResult Index()
        {
            var userId = User.Identity.GetUserId();
            var timeTable = db.TimeTables
                .Include(t => t.TimeSlots)
                .FirstOrDefault(t => t.UserId == userId);

            if (timeTable == null)
            {
                timeTable = new TimeTable { UserId = userId };
                db.TimeTables.Add(timeTable);
                db.SaveChanges();
            }

            return View(timeTable);
        }

        public ActionResult AddTimeSlot(int timeTableId, string Day, string StartTime, string EndTime, string Subject, string Location)
        {
            var dayEnum = (DayOfWeek)Enum.Parse(typeof(DayOfWeek), Day);
            var start = TimeSpan.Parse(StartTime);
            var end = TimeSpan.Parse(EndTime);

            if (end <= start)
            {
                ModelState.AddModelError("", "End time must be after start time.");
                return RedirectToAction("Index");
            }

            var conflicts = db.TimeSlots.Any(slot =>
                slot.TimeTableId == timeTableId &&  // also check by timetable
                slot.Day == dayEnum &&
                !(slot.EndTime <= start || slot.StartTime >= end)
            );

            if (conflicts)
            {
                TempData["TimeConflict"] = "The selected time overlaps with an existing activity.";
                return RedirectToAction("Index");
            }

            var newSlot = new TimeSlot
            {
                TimeTableId = timeTableId,   // Set the foreign key here
                Day = dayEnum,
                StartTime = start,
                EndTime = end,
                Subject = Subject,
                Location = Location
            };

            db.TimeSlots.Add(newSlot);
            db.SaveChanges();

            return RedirectToAction("Index");
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteTimeSlot(int id)
        {
            var slot = db.TimeSlots.Find(id);
            if (slot != null)
            {
                db.TimeSlots.Remove(slot);
                db.SaveChanges();
            }
            return RedirectToAction("Index"); // Or wherever you want to go after deleting
        }



        protected override void Dispose(bool disposing)
        {
            if (disposing) db.Dispose();
            base.Dispose(disposing);
        }


    }
}