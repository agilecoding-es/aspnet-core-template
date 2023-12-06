using Microsoft.AspNetCore.Mvc;
using System.Globalization;
using System.Text.RegularExpressions;
using Template.Common;
using Template.MvcWebApp.Localization;

namespace Template.MvcWebApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ICultureHelper cultureHelper;
        private readonly ILogger<HomeController> logger;

        public HomeController(ICultureHelper cultureHelper, ILogger<HomeController> logger)
        {
            this.cultureHelper = cultureHelper;
            this.logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [HttpGet]
        public IActionResult ChangeLanguage(string culture, string returnUrl)
        {
            //var isUserLogged = _userSession.IdCompanyUser > 0;
            var isUserLogged = false;

            CultureInfo cultureInfo = null;
            CultureInfo uiCultureInfo = null;
            if (Request.Cookies.ContainsKey(Constants.Configuration.Cookies.CultureCookieName))
            {
                var regex = new Regex(RegExPatterns.Culture.CultureCookie);
                var match = regex.Match(Request.Cookies[Constants.Configuration.Cookies.CultureCookieName]);
                var currentCulture = match?.Groups[1]?.Value;
                cultureInfo = cultureHelper.GetCulture(currentCulture);
            }
            else
            {
                //TODO: Si está loggeado buscar configuración
                cultureInfo = !isUserLogged ?
                                  cultureHelper.GetDefaultCulture() :
                                  cultureHelper.GetDefaultCulture();
            }

            //TODO: Si está loggeado buscar configuración
            uiCultureInfo = !isUserLogged ?
                                cultureHelper.GetUICulture(culture) :
                                cultureHelper.GetUICulture(culture);

            cultureHelper.SetCultureCookie(Response, cultureInfo, uiCultureInfo);
            cultureHelper.SetCulture(cultureInfo, uiCultureInfo);

            return LocalRedirect(returnUrl);
        }
    }
}