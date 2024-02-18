using Template.Configuration;

namespace Template.WebApp.Middlewares
{
    public static class MiddlewareExtensions
    {
        public static IApplicationBuilder UseLogExceptions(this IApplicationBuilder applicationBuilder, Action<LoggingExceptionsOptions> configure = null)
        {
            if (configure != null)
            {
                var loggingExceptionsOptions = new LoggingExceptionsOptions();
                configure(loggingExceptionsOptions);

                return applicationBuilder.UseMiddleware<LogExceptionMiddleware>(loggingExceptionsOptions);
            }
            else
            {
                return applicationBuilder.UseMiddleware<LogExceptionMiddleware>();
            }
        }
    }
}
