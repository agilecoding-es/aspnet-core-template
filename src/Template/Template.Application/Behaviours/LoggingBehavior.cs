using MediatR;
using Microsoft.Extensions.Logging;
using System.Diagnostics;

namespace Template.Application.Behaviours
{
    public class LoggingBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    {
        private readonly ILogger _logger;

        public LoggingBehavior(ILogger<LoggingBehavior<TRequest, TResponse>> logger)
        {
            _logger = logger;
        }

        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            var requestName = request.GetType().Namespace.Replace("Template.Application.Features.", string.Empty);

            _logger.LogInformation($"[{DateTime.UtcNow}] Start request - {requestName}");
            var timer = Stopwatch.StartNew();
            var response = await next();
            timer.Stop();
            _logger.LogInformation($"[{DateTime.UtcNow}] Request finish - {requestName} | {timer.ElapsedMilliseconds}");

            return response;
        }
    }
}
