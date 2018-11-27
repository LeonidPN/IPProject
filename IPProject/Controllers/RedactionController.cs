using IPProject.Models;
using IPProject.Services;
using System;
using System.Web.Mvc;

namespace IPProject.Controllers
{
    public class RedactionController : Controller
    {
        [HttpGet]
        public ActionResult NewsList()
        {
            if (Session["userId"] != null)
            {
                return View();
            }
            else
            {
                return Redirect("/Home/Authorization");
            }
        }

        [HttpGet]
        public ActionResult NewsAdd()
        {
            if (Session["userId"] != null)
            {
                return View();
            }
            else
            {
                return Redirect("/Home/Authorization");
            }
        }

        [HttpGet]
        public ActionResult NewsAdd(int id)
        {
            try
            {
                NewsService service = new NewsService();
                News c = service.GetElement(id);
                ViewBag.Id = id;
                ViewBag.Title = c.Title;
                ViewBag.Description = c.Description;
                ViewBag.Body = c.Body;
                CategoryService serviceC = new CategoryService();
                ViewBag.Category = serviceC.GetList();
            }
            catch (Exception ex)
            {

            }
            if (Session["userId"] != null)
            {
                return View();
            }
            else
            {
                return Redirect("/Home/Authorization");
            }
        }

        [HttpGet]
        public ActionResult CategoryList()
        {
            if (Session["userId"] != null)
            {
                return View();
            }
            else
            {
                return Redirect("/Home/Authorization");
            }
        }

        [HttpGet]
        public ActionResult CategoryAdd()
        {
            if (Session["userId"] != null)
            {
                return View();
            }
            else
            {
                return Redirect("/Home/Authorization");
            }
        }

        [HttpGet]
        public ActionResult CategoryAdd(int id)
        {
            try
            {
                CategoryService service = new CategoryService();
                Category c = service.GetElement(id);
                ViewBag.Id = id;
                ViewBag.Title = c.Title;
                ViewBag.Description = c.Description;
            }
            catch (Exception ex)
            {

            }
            if (Session["userId"] != null)
            {
                return View();
            }
            else
            {
                return Redirect("/Home/Authorization");
            }
        }

        [HttpGet]
        public ActionResult UserList()
        {
            if (Session["userId"] != null)
            {
                return View();
            }
            else
            {
                return Redirect("/Home/Authorization");
            }
        }

        [HttpGet]
        public ActionResult UserAdd()
        {
            if (Session["userId"] != null)
            {
                return View();
            }
            else
            {
                return Redirect("/Home/Authorization");
            }
        }

        [HttpGet]
        public ActionResult UserAdd(int id)
        {
            try
            {
                UserService service = new UserService();
                User c = service.GetElement(id);
                ViewBag.Id = id;
                ViewBag.Login = c.Login;
                ViewBag.Password = c.Password;
                ViewBag.FirstName = c.FirstName;
                ViewBag.LastName = c.LastName;
            }
            catch (Exception ex)
            {

            }
            if (Session["userId"] != null)
            {
                return View();
            }
            else
            {
                return Redirect("/Home/Authorization");
            }
        }
    }
}