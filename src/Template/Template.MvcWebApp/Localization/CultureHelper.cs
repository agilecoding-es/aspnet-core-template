using System.Globalization;
using Microsoft.AspNetCore.Localization;
using Microsoft.Extensions.Options;
using Template.Configuration;
using static Template.Configuration.Constants;

namespace Template.MvcWebApp.Localization
{
    public class CultureHelper : ICultureHelper
    {
        private readonly AppSettings _settings;
        private readonly RequestLocalizationOptions _localizationOptions;

        public CultureHelper(
            IOptions<AppSettings> settings,
            IOptions<RequestLocalizationOptions> localizationOptions)
        {
            _settings = settings?.Value ?? throw new ArgumentNullException(nameof(settings));
            _localizationOptions = localizationOptions?.Value ?? throw new ArgumentNullException(nameof(localizationOptions));
        }

        public CultureInfo GetCulture(string culture)
        {
            CultureInfo cultureInfo = null;

            cultureInfo = _localizationOptions.SupportedCultures.FirstOrDefault(c => c.ToString() == culture);

            return cultureInfo;
        }


        public CultureInfo GetUICulture(string culture)
        {
            CultureInfo cultureInfo = null;

            cultureInfo = _localizationOptions.SupportedUICultures.First(c => c.ToString() == _settings.SupportedCultures.GetValidCulture(culture));

            return cultureInfo;
        }


        public CultureInfo GetDefaultCulture()
        {
            CultureInfo cultureInfo = null;

            cultureInfo = _localizationOptions.SupportedCultures.First(c => c.ToString() == _settings.SupportedCultures.DefaultCulture);

            return cultureInfo;
        }

        public void SetCulture(CultureInfo culture) => SetCulture(culture, culture);

        public void SetCulture(CultureInfo culture, CultureInfo uiCulture)
        {
            CultureInfo.CurrentCulture = culture;
            CultureInfo.DefaultThreadCurrentCulture = culture;

            CultureInfo.CurrentUICulture = uiCulture;
            CultureInfo.DefaultThreadCurrentUICulture = uiCulture;
        }

        public void SetCultureCookie(HttpResponse response, CultureInfo culture, DateTimeOffset? expires = null) =>
            SetCultureCookie(response, culture, culture, expires);

        public void SetCultureCookie(HttpResponse response, CultureInfo culture, CultureInfo uiCulture, DateTimeOffset? expires = null)
        {
            expires = expires ?? DateTimeOffset.UtcNow.AddDays(_settings.SupportedCultures.CookieLifeTimeDays);

            response.Cookies.Append(
                Cookies.CULTURE_COOKIE ,
                CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(culture, uiCulture)),
                    new CookieOptions
                    {
                        Expires = expires
                    }
            );
        }
    }
}
