using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Globalization;
using System.Text.RegularExpressions;
using Template.Configuration;
using Template.MvcWebApp.Localization;
using Template.MvcWebApp.Models;
using static Template.Configuration.Constants.Configuration;

namespace Template.MvcWebApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ICultureHelper _cultureHelper;
        private readonly ILogger<HomeController> _logger;

        public HomeController(ICultureHelper cultureHelper, ILogger<HomeController> logger)
        {
            _cultureHelper = cultureHelper;
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }


        [HttpGet]
        public IActionResult ChangeLanguage(string culture, string returnUrl)
        {
            //var isUserLogged = _userSession.IdCompanyUser > 0;
            var isUserLogged = false;

            CultureInfo cultureInfo = null;
            CultureInfo uiCultureInfo = null;
            if (Request.Cookies.ContainsKey(Cookies.CULTURE_COOKIE))
            {
                var regex = new Regex(RegExPatterns.Culture.CULTURE_COOKIE);
                var match = regex.Match(Request.Cookies[Cookies.CULTURE_COOKIE]);
                var currentCulture = match?.Groups[1]?.Value;
                cultureInfo = _cultureHelper.GetCulture(currentCulture);
            }
            else
            {
                //TODO: Si está loggeado buscar configuración
                cultureInfo = !isUserLogged ?
                                  _cultureHelper.GetDefaultCulture() :
                                  _cultureHelper.GetDefaultCulture();
            }

            //TODO: Si está loggeado buscar configuración
            uiCultureInfo = !isUserLogged ?
                                _cultureHelper.GetUICulture(culture) :
                                _cultureHelper.GetUICulture(culture);

            _cultureHelper.SetCultureCookie(Response, cultureInfo, uiCultureInfo);
            _cultureHelper.SetCulture(cultureInfo, uiCultureInfo);

            return LocalRedirect(returnUrl);
        }
    }
}