using System.Web.Mvc;

namespace IPProject.Controllers
{
    public class MessageController : Controller
    {
        [HttpGet]
        public ActionResult MessageShow(string message, string href)
        {
            ViewBag.Message = message;
            ViewBag.Href = href;
            return View();
        }
    }
}