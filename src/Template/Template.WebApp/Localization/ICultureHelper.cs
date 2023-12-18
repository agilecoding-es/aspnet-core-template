using System.Globalization;

namespace Template.WebApp.Localization
{
    public interface ICultureHelper
    {
        CultureInfo GetCulture(string culture);

        CultureInfo GetUICulture(string culture);

        CultureInfo GetDefaultCulture();
        void SetCulture(CultureInfo culture);
        void SetCulture(CultureInfo culture, CultureInfo uiCulture);

        void SetCultureCookie(HttpResponse response, CultureInfo culture, DateTimeOffset? expires = null);
        void SetCultureCookie(HttpResponse response, CultureInfo culture, CultureInfo uiCulture, DateTimeOffset? expires = null);
    }
}
