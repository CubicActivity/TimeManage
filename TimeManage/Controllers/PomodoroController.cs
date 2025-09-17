using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TimeManage.Controllers
{
    [Authorize]
    public class PomodoroController : Controller
    {
        // GET:
        // 
        public ActionResult Index()
        {
            return View();
        }
    }
}