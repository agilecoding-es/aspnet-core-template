using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Template.Application.Behaviours
{
    public class LogginBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    {
        private readonly ILogger _logger;

        public LogginBehavior(ILogger<LogginBehavior<TRequest, TResponse>> logger)
        {
            _logger = logger;
        }

        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            var requestName = request!.GetType().Name;

            _logger.LogInformation($"[{DateTime.UtcNow}] Start request - {requestName}");
            var timer = Stopwatch.StartNew();
            var response = await next();
            timer.Stop();
            _logger.LogInformation($"[{DateTime.UtcNow}] Request finish - {requestName} | {timer.ElapsedMilliseconds}");

            return response;
        }
    }
}
