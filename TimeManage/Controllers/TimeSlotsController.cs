using System;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using TimeManage.Models;

namespace TimeManage.Controllers
{
    [Authorize]
    public class TimeslotsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Timeslots/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var timeslot = db.TimeSlots.Find(id);
            if (timeslot == null)
                return HttpNotFound();

            return View(timeslot);
        }

        // GET: Timeslots/Create
        public ActionResult Create(int timetableId)
        {
            var timeslot = new TimeSlot { TimeTableId = timetableId };
            return View(timeslot);
        }

        // POST: Timeslots/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Day,StartTime,EndTime,Subject,Location,Color,Txtcolor,TimeTableId")] TimeSlot timeslot)
        {
            if (ModelState.IsValid)
            {
                db.TimeSlots.Add(timeslot);
                db.SaveChanges();
                // redirect to user's TimeTable
                return RedirectToAction("Index", "TimeTable");
            }

            return View(timeslot);
        }

        // GET: Timeslots/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var timeslot = db.TimeSlots.Find(id);
            if (timeslot == null)
                return HttpNotFound();

            return View(timeslot);
        }

        // POST: Timeslots/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Day,StartTime,EndTime,Subject,Location,Color,Txtcolor,TimeTableId")] TimeSlot timeslot)
        {
            if (ModelState.IsValid)
            {
                db.Entry(timeslot).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index", "TimeTable");
            }
            return View(timeslot);
        }

        // GET: Timeslots/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var timeslot = db.TimeSlots.Find(id);
            if (timeslot == null)
                return HttpNotFound();

            return View(timeslot);
        }

        // POST: Timeslots/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            var timeslot = db.TimeSlots.Find(id);
            if (timeslot == null)
                return HttpNotFound();

            db.TimeSlots.Remove(timeslot);
            db.SaveChanges();

            // redirect to user's TimeTable
            return RedirectToAction("Index", "TimeTable");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
                db.Dispose();
            base.Dispose(disposing);
        }
    }
}
