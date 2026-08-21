using Microsoft.AspNetCore.Mvc;
using PFCWebAPP.Filters;

namespace PFCWebAPP.Controllers
{
    [PFCExceptionFilter]
    public class UnAuthorisedController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult UnAuthorisedUserRole()
        {
            return View();
        }

        public ActionResult NotAvailable(string ErrorMessage)
        {
            var message = ErrorMessage;
            ViewBag.Message = message;
            return View();
        }
    }
}
