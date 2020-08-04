using Microsoft.AspNetCore.Mvc;

namespace MyProject.Web.Controllers
{
    public class ErrorController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
