using MediatR;
using Microsoft.Extensions.Logging;
using System.Drawing;
using Template.Domain.Entities.Shared;
using Template.Infrastructure.Caching.Abastractions;
using Template.Infrastructure.Caching.Service;

namespace Template.Infrastructure.Caching.Mediator.Behavior
{
    public class CachingBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    {
        private readonly ICacheService cache;
        private readonly ILogger logger;

        public CachingBehavior(ICacheService cache, ILogger<CachingBehavior<TRequest, TResponse>> logger)
        {
            this.cache = cache ?? throw new ArgumentNullException(nameof(cache));
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            var cacheableRequest = request as ICacheable;
            if (cacheableRequest is null)
            {
                return await next();
            }

            var requestName = cacheableRequest.GetType().Name;

            logger.LogInformation($"[{DateTime.UtcNow}] Caching request - {requestName}");

            var response = await cache.GetAsync<TResponse>($"-{cacheableRequest.CacheKey}", cancellationToken);

            if (!EqualityComparer<TResponse>.Default.Equals(response, default))
            {
                logger.LogInformation($"[{DateTime.UtcNow}] Returning value from cache key - {cacheableRequest.CacheKey}");

                return response;
            }

            response = await next();

            if (response is Result result && result.IsSuccess)
            {
                logger.LogInformation($"[{DateTime.UtcNow}] Returning value from persistence and saving to cache service with key - {cacheableRequest.CacheKey}");
                await cache.SetAsync($"-{cacheableRequest.CacheKey}", response, cancellationToken);
            }
            return response;
        }
    }
}
