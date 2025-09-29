using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Drawing;
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

        public ActionResult Edit(int? id)
        {
            if (id == null) return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);

            var timeSlot = db.TimeSlots.Find(id);
            if (timeSlot == null) return HttpNotFound();

            timeSlot.StartTime = TimeSpan.FromHours(timeSlot.StartTime.Hours);
            timeSlot.EndTime = TimeSpan.FromHours(timeSlot.EndTime.Hours);

            var otherSlots = db.TimeSlots
                .Where(s => s.TimeTableId == timeSlot.TimeTableId && s.Id != timeSlot.Id)
                .Select(s => new {
                    Day = (int)s.Day,
                    StartHour = s.StartTime.Hours,
                    EndHour = s.EndTime.Hours,
                    Subject = s.Subject
                })
                .ToList();

            ViewBag.OtherSlots = otherSlots;
            return View(timeSlot);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,TimeTableId,Day,Subject,Location,Color,Txtcolor")] TimeSlot slot, int StartHour, int EndHour)
        {
            var start = TimeSpan.FromHours(StartHour);
            var end = TimeSpan.FromHours(EndHour);

            if (end <= start)
            {
                ModelState.AddModelError("", "End hour must be later than start hour.");
                slot.StartTime = start;
                slot.EndTime = end;
                return View(slot);
            }

            var conflicts = db.TimeSlots
                .Where(s => s.TimeTableId == slot.TimeTableId
                            && s.Id != slot.Id
                            && s.Day == slot.Day
                            && !(s.EndTime <= start || s.StartTime >= end))
                .ToList();

            if (conflicts.Any())
            {
                ModelState.AddModelError("", "The selected time overlaps with existing activity(ies).");
                ViewBag.Conflicts = conflicts
                .AsEnumerable() // force in-memory
                .Select(c => new Dictionary<string, object>{
                    { "DayName", c.Day.ToString() },
                    { "StartHour", c.StartTime.Hours },
                    { "EndHour", c.EndTime.Hours },
                    { "Subject", c.Subject }
                })
                .ToList();

                slot.StartTime = start;
                slot.EndTime = end;
                return View(slot);
            }

            if (ModelState.IsValid)
            {
                var existing = db.TimeSlots.Find(slot.Id);
                if (existing == null) return HttpNotFound();

                existing.Day = slot.Day;
                existing.StartTime = start;
                existing.EndTime = end;
                existing.Subject = slot.Subject;
                existing.Location = slot.Location;
                existing.Color = slot.Color;
                existing.Txtcolor = slot.Txtcolor;

                db.SaveChanges();
                return RedirectToAction("Index", new { id = existing.TimeTableId });
            }
            slot.StartTime = start;
            slot.EndTime = end;
            return View(slot);
        }


        public ActionResult AddTimeSlot(int timeTableId, string Day, string StartTime, string EndTime, string Subject, string Location, string color, string txtcolor)
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
                slot.TimeTableId == timeTableId &&
                slot.Day == dayEnum &&
                !(slot.EndTime <= start || slot.StartTime >= end)
            );

            if (conflicts)
            {
                var overlapping = db.TimeSlots
                .Where(s => s.TimeTableId == timeTableId && s.Day == dayEnum && !(s.EndTime <= start || s.StartTime >= end)).AsEnumerable().Select(s => new Dictionary<string, object>{
                    { "DayName", s.Day.ToString() },
                    { "StartHour", s.StartTime.Hours },
                    { "EndHour", s.EndTime.Hours },
                    { "Subject", s.Subject }
                }).ToList();

                TempData["Conflicts"] = overlapping;
                return RedirectToAction("Index");
            }
            var newSlot = new TimeSlot
            {
                TimeTableId = timeTableId,
                Day = dayEnum,
                StartTime = start,
                EndTime = end,
                Subject = Subject,
                Location = Location,
                Color = string.IsNullOrWhiteSpace(color) ? "#FFFFFF" : color,
                Txtcolor = string.IsNullOrWhiteSpace(txtcolor) ? "#000000" : txtcolor
            };

            db.TimeSlots.Add(newSlot);
            db.SaveChanges();

            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult DeleteTimeSlot(int id)
        {
            var slot = db.TimeSlots.Find(id);
            if (slot != null)
            {
                db.TimeSlots.Remove(slot);
                db.SaveChanges();
            }
            return RedirectToAction("Index");
        }
        protected override void Dispose(bool disposing)
        {
            if (disposing) db.Dispose();
            base.Dispose(disposing);
        }

    }
}