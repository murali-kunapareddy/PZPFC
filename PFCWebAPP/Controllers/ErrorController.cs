using PFCWebAPP.Models;
using Microsoft.AspNetCore.Mvc;
using PFCWebAPP.Filters;

namespace PFCWebAPP.Controllers
{
    [PFCExceptionFilter]
    public class ErrorController : Controller
    {
        string message = "";
        public IActionResult Index()
        {
            return View();
        }

        [Route("Error/{StatusCode}")]
        public ActionResult HTTPStatusCodeHandler(int StatusCode)
        {
            var vm = new ErrorViewModel();
            //Response.StatusCode = 500;
            vm.ErrorView = "_Error500";


            switch (StatusCode)
            {
                case 401:
                    message = $"Access Denied!";
                    ViewBag.Message = message;
                    vm.ErrorView = "_Error401";
                    break;
                case 403:
                    message = $"MaintenanceMode!";
                    ViewBag.Message = message;
                    vm.ErrorView = "_Error403";
                    break;
                case 404:
                    message = $"Opps....Page you are looking for is not found!";
                    ViewBag.Message = message;
                    vm.ErrorView = "_Error404";
                    break;
                default:
                    message = $"Internal Server Error, Please Contact Support Team!!!";
                    ViewBag.Message = message;
                    vm.ErrorView = "_Error500";

                    break;
            }

            return View("Index", vm);


        }

        public ActionResult Info(int StatusCode)
        {
            var vm = new ErrorViewModel();
            //Response.StatusCode = 500;
            vm.ErrorView = "_Error500";


            switch (StatusCode)
            {
                case 401:
                    message = $"Access Denied!";
                    ViewBag.Message = message;
                    vm.ErrorView = "_Error401";
                    break;
                case 403:
                    message = $"MaintenanceMode!";
                    ViewBag.Message = message;
                    vm.ErrorView = "_Error403";
                    break;
                case 404:
                    message = $"Opps....Page you are looking for is not found!";
                    ViewBag.Message = message;
                    vm.ErrorView = "_Error404";
                    break;
                default:
                    message = $"Internal Server Error, Please Contact Support Team!!!";
                    ViewBag.Message = message;
                    vm.ErrorView = "_Error500";

                    break;
            }

            return View("Index", vm);


        }
    }
}
