using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace IPProject.Controllers
{
    public class NewsController : Controller
    {
        [HttpGet]
        public ActionResult Interesting()
        {
            return View();
        }

        [HttpGet]
        public ActionResult News()
        {
            return View();
        }

        [HttpGet]
        public ActionResult NewsInCategory()
        {
            return View();
        }
    }
}