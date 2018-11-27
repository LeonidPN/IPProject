using IPProject.Services;
using System.Web.Mvc;

namespace IPProject.Controllers
{
    public class InformationController : Controller
    {
        [HttpGet]
        public ActionResult About()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Contact()
        {
            return View();
        }

        [HttpPost]
        public void Contact(string name, string email, string message)
        {
            MailService service = new MailService();
            service.SendEmail(name, email, message);
            Redirect("/Information/Contact");
        }
    }
}