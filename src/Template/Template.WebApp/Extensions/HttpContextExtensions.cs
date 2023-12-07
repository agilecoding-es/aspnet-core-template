using Microsoft.AspNetCore.Localization;

namespace Template.WebApp.Extensions
{
    public static class HttpContextExtensions
    {
        public static IRequestCultureFeature GetRequestCultureFeature(this HttpContext context) => context.Features.Get<IRequestCultureFeature>();

    }
}
