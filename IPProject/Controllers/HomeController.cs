using IPProject.Models;
using IPProject.Services;
using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace IPProject.Controllers
{
    public class HomeController : Controller
    {
        [HttpGet]
        public ActionResult Index()
        {
            try
            {
                CategoryService service = new CategoryService(Server.MapPath("~/Content/Upload/Entities/"));
                List<Category> categories = service.GetList();
                ViewBag.Categories = categories;
            }
            catch (Exception ex)
            {
                string message = "";
                while (ex != null)
                {
                    message = ex.Message;
                    ex = ex.InnerException;
                }
                message = message.Replace('\n', ' ');
                return Redirect("/Message/MessageShow/?message=" + message + "&href=" + Request.Url);
            }
            return View();
        }

        [HttpGet]
        public ActionResult Authorization()
        {
            Session.Clear();
            return View();
        }

        [HttpPost]
        public ActionResult Authorization(string login, string pass)
        {
            try
            {
                UserService service = new UserService(Server.MapPath("~/Content/Upload/Entities/"));
                if (service.Authorization(login, pass) != null)
                {
                    Session.Add("userId", service.Authorization(login, pass).Id);
                    return Redirect("/Redaction/NewsList");
                }
            }
            catch (Exception ex)
            {
                string message = "";
                while (ex != null)
                {
                    message = ex.Message;
                    ex = ex.InnerException;
                }
                message = message.Replace('\n', ' ');
                return Redirect("/Message/MessageShow/?message=" + message + "&href=" + Request.Url);
            }
            return Redirect("/Home/Authorization");
        }
    }
}