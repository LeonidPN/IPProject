using IPProject.Models;
using IPProject.Services;
using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace IPProject.Controllers
{
    public class NewsController : Controller
    {
        [HttpGet]
        public ActionResult Interesting()
        {
            try
            {
                NewsService service = new NewsService(Server.MapPath("~/Content/Upload/Entities/"));
                List<News> news = service.GetInteresting();
                ViewBag.News = news;
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
        public ActionResult News(int id)
        {
            try
            {
                NewsService service = new NewsService(Server.MapPath("~/Content/Upload/Entities/"));
                News news = service.GetElement(id);
                string str = news.Body;
                string[] b = str.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
                List<string> n = new List<string>();
                for (int i = 0; i < b.Length; i++)
                {
                    n.Add(b[i]);
                }
                service.IncreaseViews(id);
                ViewBag.News = news;
                ViewBag.Body = n;
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
        public ActionResult NewsInCategory(int id)
        {
            try
            {
                NewsService service = new NewsService(Server.MapPath("~/Content/Upload/Entities/"));
                CategoryService serviceC = new CategoryService(Server.MapPath("~/Content/Upload/Entities/"));
                List<News> news = service.GetListByCategory(id);
                ViewBag.News = news;
                ViewBag.Category = serviceC.GetElement(id).Title;
                ViewBag.CategoryId = id;
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

        public ActionResult AddItems(int start, int count, int id)
        {
            try
            {
                NewsService service = new NewsService(Server.MapPath("~/Content/Upload/Entities/"));
                List<News> news = service.GetListByCategory(id);
                List<News> n = new List<News>();
                for (int i = start; i < news.Count && count > 0; i++)
                {
                    count--;
                    n.Add(news[i]);
                }
                return PartialView(n);
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
        }
    }
}