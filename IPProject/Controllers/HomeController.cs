using IPProject.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace IPProject.Controllers
{
    public class HomeController : Controller
    {
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Authorization()
        {
            Session.Clear();
            return View();
        }

        [HttpPost]
        public void Authorization(string login, string pass)
        {
            try
            {
                UserService service = new UserService();
                if (service.Authorization(login, pass) != null)
                {
                    Session.Add("userId", service.Authorization(login, pass).Id);
                    Redirect("/Redaction/NewsList");
                }
            }
            catch (Exception ex)
            {
                Redirect("/Home/Authorization");
            }
        }
    }
}