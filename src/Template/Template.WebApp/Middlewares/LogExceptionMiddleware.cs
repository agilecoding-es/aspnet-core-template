using Microsoft.Extensions.Options;
using Template.Configuration;

namespace Template.WebApp.Middlewares
{
    public class LogExceptionMiddleware
    {
        private readonly RequestDelegate next;
        private readonly LogMiddleware options;
        private readonly ILogger<LogExceptionMiddleware> logger;

        public LogExceptionMiddleware(RequestDelegate next, IOptions<LogMiddleware> options, ILogger<LogExceptionMiddleware> logger)
        {
            this.next = next ?? throw new ArgumentNullException(nameof(next));
            this.options = options.Value ?? throw new ArgumentNullException(nameof(options));
            this.logger = logger ?? throw new ArgumentNullException(nameof(next));
        }

        public LogExceptionMiddleware(RequestDelegate next, LogMiddleware options, ILogger<LogExceptionMiddleware> logger)
        {
            this.next = next ?? throw new ArgumentNullException(nameof(next));
            this.options = options ?? throw new ArgumentNullException(nameof(options));
            this.logger = logger ?? throw new ArgumentNullException(nameof(next));
        }

        public async Task Invoke(HttpContext httpContext)
        {
            try
            {
                await next(httpContext);
            }
            catch (Exception ex)
            {
                if (options.Enabled || !options.Rethrow)
                    logger.LogError(ex, ex.Message);

                if (options.Rethrow)
                    throw;
            }
        }

    }
}
