using PFCWebAPP.Filters;
using Microsoft.AspNetCore.Mvc;


namespace PFCWebAPP.Controllers
{
    [PFCAuthFilter]
    public class BaseController : Controller
    {

        //public class EmptyStringModelBinder : IModelBinderProvider
        //{
        //    public IModelBinder GetBinder(ModelBinderProviderContext context)
        //    {
               
        //    }
        //}

    }
}
