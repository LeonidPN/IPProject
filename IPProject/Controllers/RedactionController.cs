using IPProject.Models;
using IPProject.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Web;
using System.Web.Mvc;

namespace IPProject.Controllers
{
    public class RedactionController : Controller
    {
        [HttpGet]
        public ActionResult NewsList()
        {
            try
            {
                NewsService service = new NewsService(Server.MapPath("~/Content/Upload/Entities/"));
                List<News> news = service.GetList();
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
            if (Session["userId"] != null)
            {
                return View();
            }
            else
            {
                return Redirect("/Home/Authorization");
            }
        }

        [HttpPost]
        public ActionResult NewsList(string button, int? id)
        {
            if (Session["userId"] != null)
            {
                if (id.HasValue)
                {
                    if (button == "Изменить")
                    {
                        return Redirect("/Redaction/NewsAdd/?id=" + id.Value);
                    }
                    else
                    {
                        try
                        {
                            NewsService service = new NewsService(Server.MapPath("~/Content/Upload/Entities/"));
                            service.DelElement(id.Value);
                            return Redirect("/Redaction/NewsList");
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
                else
                {
                    return Redirect("/Redaction/NewsAdd");
                }
            }
            else
            {
                return Redirect("/Home/Authorization");
            }
        }

        [HttpGet]
        public ActionResult NewsAdd(int? id)
        {
            try
            {
                if (id.HasValue)
                {
                    NewsService service = new NewsService(Server.MapPath("~/Content/Upload/Entities/"));
                    News c = service.GetElement(id.Value);
                    ViewBag.Id = id;
                    ViewBag.Title = c.Title;
                    ViewBag.Description = c.Description;
                    ViewBag.Body = c.Body;
                    ViewBag.DateCreate = c.DateCreate.ToString();
                    ViewBag.NumberOfViews = c.NumberOfViews;
                    CategoryService serviceC = new CategoryService(Server.MapPath("~/Content/Upload/Entities/"));
                    ViewBag.Category = serviceC.GetList();
                }
                else
                {
                    CategoryService serviceC = new CategoryService(Server.MapPath("~/Content/Upload/Entities/"));
                    ViewBag.Category = serviceC.GetList();
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
            if (Session["userId"] != null)
            {
                return View();
            }
            else
            {
                return Redirect("/Home/Authorization");
            }
        }

        [HttpPost]
        public ActionResult NewsAdd(int? id, string date, int? views, int category, string title, string shortDesc, string body, HttpPostedFileBase image)
        {
            if (Session["userId"] == null)
            {
                return Redirect("/Home/Authorization");
            }
            try
            {
                string path = null;
                if (image != null)
                {
                    path = GetImageUrl(Path.GetExtension(image.FileName)) + Path.GetExtension(image.FileName);
                    image.SaveAs(path);
                    path = Path.GetFileName(path);
                }
                NewsService service = new NewsService(Server.MapPath("~/Content/Upload/Entities/"));
                if (id.HasValue)
                {
                    service.UpdElement(new News
                    {
                        Title = title,
                        Body = body,
                        CategoryId = category,
                        DateCreate = DateTime.Parse(date),
                        Description = shortDesc,
                        Id = id.Value,
                        ImageUrl = path,
                        NumberOfViews = views.Value,
                        UserId = (int)Session["userId"]
                    });
                }
                else
                {
                    service.Add(new News
                    {
                        Title = title,
                        Body = body,
                        CategoryId = category,
                        Description = shortDesc,
                        ImageUrl = path,
                        UserId = (int)Session["userId"]
                    });
                }
                return Redirect("/Message/MessageShow/?message=" + "Успешно сохранено" + "&href=" + Request.Url);
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

        [HttpGet]
        public ActionResult CategoryList()
        {
            try
            {
                CategoryService service = new CategoryService(Server.MapPath("~/Content/Upload/Entities/"));
                List<Category> cat = service.GetList();
                ViewBag.Category = cat;
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
            if (Session["userId"] != null)
            {
                return View();
            }
            else
            {
                return Redirect("/Home/Authorization");
            }
        }

        [HttpPost]
        public ActionResult CategoryList(string button, int? id)
        {
            if (Session["userId"] != null)
            {
                if (id.HasValue)
                {
                    if (button == "Изменить")
                    {
                        return Redirect("/Redaction/CategoryAdd/?id=" + id.Value);
                    }
                    else
                    {
                        try
                        {
                            CategoryService service = new CategoryService(Server.MapPath("~/Content/Upload/Entities/"));
                            service.DelElement(id.Value);
                            return Redirect("/Redaction/CategoryList");
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
                else
                {
                    return Redirect("/Redaction/CategoryAdd");
                }
            }
            else
            {
                return Redirect("/Home/Authorization");
            }
        }

        [HttpGet]
        public ActionResult CategoryAdd(int? id)
        {
            try
            {
                if (id.HasValue)
                {
                    CategoryService service = new CategoryService(Server.MapPath("~/Content/Upload/Entities/"));
                    Category c = service.GetElement(id.Value);
                    ViewBag.Id = id;
                    ViewBag.Title = c.Title;
                    ViewBag.Description = c.Description;
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
            if (Session["userId"] != null)
            {
                return View();
            }
            else
            {
                return Redirect("/Home/Authorization");
            }
        }

        [HttpPost]
        public ActionResult CategoryAdd(int? id, string title, string desc, HttpPostedFileBase image)
        {
            if (Session["userId"] == null)
            {
                return Redirect("/Home/Authorization");
            }
            try
            {
                string path = null;
                if (image != null)
                {
                    path = GetImageUrl(Path.GetExtension(image.FileName)) + Path.GetExtension(image.FileName);
                    image.SaveAs(path);
                    path = Path.GetFileName(path);
                }
                CategoryService service = new CategoryService(Server.MapPath("~/Content/Upload/Entities/"));
                if (id.HasValue)
                {
                    service.UpdElement(new Category
                    {
                        Description = desc,
                        Id = id.Value,
                        ImageUrl = path,
                        Title = title
                    });
                }
                else
                {
                    service.Add(new Category
                    {
                        Description = desc,
                        ImageUrl = path,
                        Title = title
                    });
                }
                return Redirect("/Message/MessageShow/?message=" + "Успешно сохранено" + "&href=" + Request.Url);
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

        [HttpGet]
        public ActionResult UserList()
        {
            try
            {
                UserService service = new UserService(Server.MapPath("~/Content/Upload/Entities/"));
                List<User> user = service.GetList();
                ViewBag.User = user;
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
            if (Session["userId"] != null)
            {
                return View();
            }
            else
            {
                return Redirect("/Home/Authorization");
            }
        }

        [HttpPost]
        public ActionResult UserList(string button, int? id)
        {
            if (Session["userId"] != null)
            {
                if (id.HasValue)
                {
                    if (button == "Изменить")
                    {
                        return Redirect("/Redaction/UserAdd/?id=" + id.Value);
                    }
                    else
                    {
                        try
                        {
                            if (Int32.Parse(Session["userId"].ToString()) == id.Value)
                            {
                                return Redirect("/Message/MessageShow/?message=" + "Невозможно удалить" + "&href=" + Request.Url);
                            }
                            else
                            {
                                UserService service = new UserService(Server.MapPath("~/Content/Upload/Entities/"));
                                service.DelElement(id.Value);
                                return Redirect("/Redaction/UserList");
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
                    }
                }
                else
                {
                    return Redirect("/Redaction/UserAdd");
                }
            }
            else
            {
                return Redirect("/Home/Authorization");
            }
        }

        [HttpGet]
        public ActionResult UserAdd(int? id)
        {
            try
            {
                if (id.HasValue)
                {
                    UserService service = new UserService(Server.MapPath("~/Content/Upload/Entities/"));
                    User c = service.GetElement(id.Value);
                    ViewBag.Id = id;
                    ViewBag.Login = c.Login;
                    ViewBag.Password = c.Password;
                    ViewBag.FirstName = c.FirstName;
                    ViewBag.LastName = c.LastName;
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
            if (Session["userId"] != null)
            {
                return View();
            }
            else
            {
                return Redirect("/Home/Authorization");
            }
        }

        [HttpPost]
        public ActionResult UserAdd(int? id, string login, string pass, string firstname, string lastname)
        {
            if (Session["userId"] == null)
            {
                return Redirect("/Home/Authorization");
            }
            try
            {
                UserService service = new UserService(Server.MapPath("~/Content/Upload/Entities/"));
                if (id.HasValue)
                {
                    service.UpdElement(new User
                    {
                        FirstName = firstname,
                        Id = id.Value,
                        LastName = lastname,
                        Login = login,
                        Password = pass
                    });
                }
                else
                {
                    service.Add(new User
                    {
                        Password = pass,
                        Login = login,
                        FirstName = firstname,
                        LastName = lastname
                    });
                }
                return Redirect("/Message/MessageShow/?message=" + "Успешно сохранено" + "&href=" + Request.Url);
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

        private string GetImageUrl(string ex)
        {
            string path = Server.MapPath("~/Content/Upload/Images/");
            int i = 0;
            while (System.IO.File.Exists(path + i + ex))
            {
                i++;
            }
            return path + i;
        }
    }
}