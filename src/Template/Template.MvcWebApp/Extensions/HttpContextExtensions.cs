using Microsoft.AspNetCore.Localization;

namespace Template.MvcWebApp.Extensions
{
    public static class HttpContextExtensions
    {
        public static IRequestCultureFeature GetRequestCultureFeature(this HttpContext context) => context.Features.Get<IRequestCultureFeature>();

    }
}
