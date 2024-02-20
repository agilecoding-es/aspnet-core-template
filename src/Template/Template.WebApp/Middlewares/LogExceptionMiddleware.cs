using Microsoft.Extensions.Options;
using Template.Configuration;

namespace Template.WebApp.Middlewares
{
    public class LogExceptionMiddleware
    {
        private readonly RequestDelegate next;
        private readonly LoggingExceptionsOptions loggingExceptionsOptions;
        private readonly ILogger<LogExceptionMiddleware> logger;

        public LogExceptionMiddleware(RequestDelegate next, IOptions<LoggingExceptionsOptions> loggingExceptionsOptions, ILogger<LogExceptionMiddleware> logger)
        {
            this.next = next ?? throw new ArgumentNullException(nameof(next));
            this.loggingExceptionsOptions = loggingExceptionsOptions.Value ?? throw new ArgumentNullException(nameof(loggingExceptionsOptions));
            this.logger = logger ?? throw new ArgumentNullException(nameof(next));
        }

        public LogExceptionMiddleware(RequestDelegate next, LoggingExceptionsOptions loggingExceptionsSettings, ILogger<LogExceptionMiddleware> logger)
        {
            this.next = next ?? throw new ArgumentNullException(nameof(next));
            this.loggingExceptionsOptions = loggingExceptionsSettings ?? throw new ArgumentNullException(nameof(loggingExceptionsSettings));
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task Invoke(HttpContext httpContext)
        {
            try
            {
                await next(httpContext);
            }
            catch (Exception ex)
            {
                if (loggingExceptionsOptions.Enabled || !loggingExceptionsOptions.Rethrow)
                    logger.LogError(ex, ex.Message);

                if (loggingExceptionsOptions.Rethrow)
                    throw;
            }
        }

    }
}
