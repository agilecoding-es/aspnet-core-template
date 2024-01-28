using Template.Configuration;

namespace Template.WebApp.Middlewares
{
    public static class MiddlewareExtensions
    {
        public static IApplicationBuilder UseLogExceptions(this IApplicationBuilder applicationBuilder, Action<LoggingOptions> configure = null)
        {
            if (configure != null)
            {
                var options = new LoggingOptions();
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
