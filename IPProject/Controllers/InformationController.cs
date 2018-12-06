using IPProject.Services;
using System;
using System.Threading.Tasks;
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
        [ValidateInput(false)]
        public ActionResult Contact()
        {
            ValidateRequest = false;
            return View();
        }

        [HttpPost]
        [ValidateInput(false)]
        public async Task<ActionResult> Contact(string name, string email, string message)
        {
            try
            {
                MailService service = new MailService();
                await service.SendEmail(name, email, message);
                return Redirect("/Information/Contact");
            }
            catch (Exception ex)
            {
                string message1 = "";
                while (ex != null)
                {
                    message1 = ex.Message;
                    ex = ex.InnerException;
                }
                message1 = message1.Replace('\n', ' ');
                return Redirect("/Message/MessageShow/?message=" + message1 + "&href=" + Request.Url);
            }
        }
    }
}