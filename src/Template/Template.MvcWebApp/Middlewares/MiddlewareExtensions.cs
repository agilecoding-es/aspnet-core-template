using Template.Configuration;

namespace Template.MvcWebApp.Middlewares
{
    public static class MiddlewareExtensions
    {
        public static IApplicationBuilder UseLogExceptions(this IApplicationBuilder applicationBuilder, Action<LogMiddleware> configure = null)
        {
            if (configure != null)
            {
                var options = new LogMiddleware();
                configure(options);

                return applicationBuilder.UseMiddleware<LogExceptionMiddleware>(options);
            }
            else
            {
                return applicationBuilder.UseMiddleware<LogExceptionMiddleware>();
            }
        }
    }
}
