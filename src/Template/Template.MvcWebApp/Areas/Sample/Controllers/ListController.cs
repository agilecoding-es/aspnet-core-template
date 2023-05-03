using Microsoft.AspNetCore.Mvc;

namespace Template.MvcWebApp.Areas.Sample.Controllers
{
    public class ListController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
