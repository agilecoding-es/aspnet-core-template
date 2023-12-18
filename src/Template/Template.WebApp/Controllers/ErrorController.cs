using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Localization;
using System.Diagnostics;
using Template.WebApp.Models;

namespace Template.WebApp.Controllers
{
    [Route("[controller]")]
    public class ErrorController : Controller
    {
        IHtmlLocalizer localizer;
        private readonly ILogger logger;

        public ErrorController(IHtmlLocalizer localizer, ILogger<ErrorController> logger)
        {
            this.localizer = localizer;
            this.logger = logger;
        }

        [Route("")]
        [Route("{statusCode}")]
        [AcceptVerbs(new[] { "GET", "HEAD", "POST" })]
        public IActionResult Error(int? statusCode = null)
        {
            switch (statusCode)
            {
                case 404:
                    return View("Error", new ErrorViewModel
                    {
                        RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier,
                        StatusCode = statusCode.ToString(),
                        Title = localizer.GetString("ErrorController_Error_404_Title"),
                        Description = localizer.GetString("ErrorController_Error_404_Description")
                    });
                case 500:
                    return View("Error", new ErrorViewModel
                    {
                        RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier,
                        StatusCode = statusCode.ToString(),
                        Title = localizer.GetString("ErrorController_Error_500_Title"),
                        Description = localizer.GetString("ErrorController_Error_500_Description")
                    });
                default:
                    return View("Error", new ErrorViewModel
                    {
                        RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier,
                        Title = localizer.GetString("ErrorController_Error_500_Title"),
                        Description = localizer.GetString("ErrorController_Error_500_Description")
                    });
            }
        }

    }
}
