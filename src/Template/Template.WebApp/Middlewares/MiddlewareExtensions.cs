using Template.Configuration;

namespace Template.WebApp.Middlewares
{
    public static class MiddlewareExtensions
    {
        public static IApplicationBuilder UseLogExceptions(this IApplicationBuilder applicationBuilder, Action<LoggingExceptionsOptions> configure = null)
        {
            if (configure != null)
            {
                var loggingExceptionsSettings = new LoggingExceptionsOptions();
                configure(loggingExceptionsSettings);

                return applicationBuilder.UseMiddleware<LogExceptionMiddleware>(loggingExceptionsSettings);
            }
            else
            {
                return applicationBuilder.UseMiddleware<LogExceptionMiddleware>();
            }
        }
    }
}
