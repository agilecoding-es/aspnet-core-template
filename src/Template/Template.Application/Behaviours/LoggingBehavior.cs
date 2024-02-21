using MediatR;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using System.Text.RegularExpressions;

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
            var requestType = request.GetType();
            var assemblyName = requestType.Assembly.GetName().Name;
            var re = new Regex($@"{assemblyName.Replace(".",@"\.")}\.(\w+(?:\.\w+)+)(?:\+\w+)?");

            var requestName = re.Match(request.GetType().FullName).Groups[1]?.Value ?? requestType.FullName;

            _logger.LogInformation($"[{DateTime.UtcNow}] Start request - {requestName}");
            var timer = Stopwatch.StartNew();
            var response = await next();
            timer.Stop();
            _logger.LogInformation($"[{DateTime.UtcNow}] Request finish - {requestName} | Elapsed ms:{timer.ElapsedMilliseconds}");

            return response;
        }
    }
}
