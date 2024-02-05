using Microsoft.Extensions.Options;
using Template.Configuration;

namespace Template.WebApp.Middlewares
{
    public class LogExceptionMiddleware
    {
        private readonly RequestDelegate next;
        private readonly LoggingExceptionsOptions loggingExceptionsSettings;
        private readonly ILogger<LogExceptionMiddleware> logger;

        public LogExceptionMiddleware(RequestDelegate next, IOptions<LoggingExceptionsOptions> options, ILogger<LogExceptionMiddleware> logger)
        {
            this.next = next ?? throw new ArgumentNullException(nameof(next));
            this.loggingExceptionsSettings = options.Value ?? throw new ArgumentNullException(nameof(options));
            this.logger = logger ?? throw new ArgumentNullException(nameof(next));
        }

        public LogExceptionMiddleware(RequestDelegate next, LoggingExceptionsOptions loggingExceptionsSettings, ILogger<LogExceptionMiddleware> logger)
        {
            this.next = next ?? throw new ArgumentNullException(nameof(next));
            this.loggingExceptionsSettings = loggingExceptionsSettings ?? throw new ArgumentNullException(nameof(loggingExceptionsSettings));
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
                if (loggingExceptionsSettings.Enabled || !loggingExceptionsSettings.Rethrow)
                    logger.LogError(ex, ex.Message);

                if (loggingExceptionsSettings.Rethrow)
                    throw;
            }
        }

    }
}
