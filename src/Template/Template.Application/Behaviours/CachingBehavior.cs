using MediatR;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Template.Application.Abastractions;

namespace Template.Application.Behaviours
{
    public class CachingBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
        where TRequest : IRequest, ICacheable
    {
        //TODO: Reemplazar por Redis
        private readonly IMemoryCache _cache;
        private readonly ILogger _logger;

        public CachingBehavior(IMemoryCache cache, ILogger<CachingBehavior<TRequest, TResponse>> logger)
        {
            _cache = cache;
            _logger = logger;
        }

        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            var requestName = request.GetType().Name;

            _logger.LogInformation($"[{DateTime.UtcNow}] Caching request - {requestName}");

            TResponse response;
            if (_cache.TryGetValue(request.CacheKey, out response))
            {
                _logger.LogInformation($"[{DateTime.UtcNow}] Returning value from cache key - {request.CacheKey}");

                return response;
            }

            response = await next();
            _cache.Set(request.CacheKey, response);

            return response;
        }
    }
}
