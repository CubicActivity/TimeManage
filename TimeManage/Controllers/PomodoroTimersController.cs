using Microsoft.AspNet.Identity;
using System;
using System.Linq;
using System.Web.Mvc;
using TimeManage.Models;

namespace TimeManage.Controllers
{
    public class PomodoroTimersController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: PomodoroTimers
        public ActionResult Index()
        {
            var timer = GetOrCreateTimer();
            var minutes = timer.Minutes;
            var seconds = timer.Seconds;
            var isRunning = timer.IsRunning;
            var displayTime = $"{minutes:D2}:{seconds:D2}";

            ViewBag.Minutes = minutes;
            ViewBag.Seconds = seconds;
            ViewBag.IsRunning = isRunning;
            ViewBag.DisplayTime = displayTime;

            return View();
        }

        [HttpPost]
        public ActionResult StartPause()
        {
            var timer = GetOrCreateTimer();

            if (timer.IsRunning)
            {
                // Pause timer
                if (timer.TimerEnd.HasValue)
                {
                    var timeLeft = timer.TimerEnd.Value - DateTime.Now;
                    if (timeLeft.TotalSeconds > 0)
                    {
                        timer.Minutes = (int)timeLeft.TotalMinutes;
                        timer.Seconds = (int)timeLeft.Seconds;
                    }
                }
                timer.IsRunning = false;
                timer.TimerEnd = null;
            }
            else
            {
                // Start timer
                var totalSeconds = (timer.Minutes * 60) + timer.Seconds;
                timer.TimerEnd = DateTime.Now.AddSeconds(totalSeconds);
                timer.IsRunning = true;
            }

            timer.UpdatedAt = DateTime.Now;
            db.SaveChanges();

            return Json(new
            {
                Minutes = timer.Minutes,
                Seconds = timer.Seconds,
                IsRunning = timer.IsRunning,
                DisplayTime = $"{timer.Minutes:D2}:{timer.Seconds:D2}"
            });
        }

        [HttpPost]
        public ActionResult Reset()
        {
            var timer = GetOrCreateTimer();

            timer.Minutes = timer.PreviousMinutes;
            timer.Seconds = timer.PreviousSeconds;
            timer.IsRunning = false;
            timer.TimerEnd = null;
            timer.UpdatedAt = DateTime.Now;

            db.SaveChanges();

            return Json(new
            {
                Minutes = timer.Minutes,
                Seconds = timer.Seconds,
                IsRunning = timer.IsRunning,
                DisplayTime = $"{timer.Minutes:D2}:{timer.Seconds:D2}"
            });
        }

        [HttpPost]
        public ActionResult SetCustomTime(int minutes, int seconds)
        {
            var timer = GetOrCreateTimer();

            timer.PreviousMinutes = minutes; 
            timer.PreviousSeconds = seconds;

            timer.Minutes = minutes;
            timer.Seconds = seconds;
            timer.IsRunning = false;
            timer.TimerEnd = null;
            timer.UpdatedAt = DateTime.Now;

            db.SaveChanges();

            return Json(new
            {
                Minutes = timer.Minutes,
                Seconds = timer.Seconds,
                IsRunning = timer.IsRunning,
                DisplayTime = $"{timer.Minutes:D2}:{timer.Seconds:D2}"
            });
        }

        [HttpGet]
        public ActionResult GetTimerStatus()
        {
            var timer = GetOrCreateTimer();

            if (timer.IsRunning && timer.TimerEnd.HasValue)
            {
                var timeLeft = timer.TimerEnd.Value - DateTime.Now;
                if (timeLeft.TotalSeconds > 0){
                    timer.Minutes = (int)timeLeft.TotalMinutes;
                    timer.Seconds = (int)timeLeft.Seconds;
                }
                else{
                    timer.Minutes = timer.PreviousMinutes;
                    timer.Seconds = timer.PreviousSeconds;
                    timer.IsRunning = false;
                    timer.TimerEnd = null;
                    db.SaveChanges();

                    return Json(new{
                        Minutes = timer.Minutes,
                        Seconds = timer.Seconds,
                        IsRunning = timer.IsRunning,
                        DisplayTime = $"{timer.Minutes:D2}:{timer.Seconds:D2}",
                        Completed = true
                    }, JsonRequestBehavior.AllowGet);

                }
            }

            return Json(new{
                Minutes = timer.Minutes,
                Seconds = timer.Seconds,
                IsRunning = timer.IsRunning,
                DisplayTime = $"{timer.Minutes:D2}:{timer.Seconds:D2}",
                Completed = false
            }, JsonRequestBehavior.AllowGet);
        }

        private PomodoroTimer GetOrCreateTimer()
        {
            string userId = null;

            if (User?.Identity?.IsAuthenticated == true)
            {
                userId = User.Identity.GetUserId();
            }

            var timer = db.PomodoroTimers.FirstOrDefault(t => t.UserId == userId);

            if (timer == null)
            {
                timer = new PomodoroTimer
                {
                    UserId = userId  
                };
                db.PomodoroTimers.Add(timer);
                db.SaveChanges();
            }
            else
            {
                if (timer.PreviousMinutes == 0 && timer.PreviousSeconds == 0)
                {
                    timer.PreviousMinutes = 25;
                    timer.PreviousSeconds = 0;
                    db.SaveChanges();
                }
            }
            return timer;
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