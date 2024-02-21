using MediatR;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using System.Text.Json;
using Template.Domain.Entities.Shared;
using Template.Infrastructure.Caching.Redis.Abastractions;

namespace Template.Infrastructure.Caching.Redis.Mediator.Behavior
{
    public class RedisCachingBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    {
        private readonly IDistributedCache cache;
        private readonly ILogger logger;

        public RedisCachingBehavior(IDistributedCache cache, ILogger<RedisCachingBehavior<TRequest, TResponse>> logger)
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

            DistributedCacheEntryOptions customOptions = null;
            if (request is ICachingOptions cachingOptions)
            {
                customOptions = cachingOptions.Options;
            }
            logger.LogInformation($"[{DateTime.UtcNow}] Caching request | Checking cache - cache-key: {cacheableRequest.CacheKey}");

            var value = await cache.GetStringAsync($"-{cacheableRequest.CacheKey}", cancellationToken);

            TResponse response = default;
            if (value is not null)
                response = JsonSerializer.Deserialize<TResponse>(value);

            if (!EqualityComparer<TResponse>.Default.Equals(response, default))
            {
                logger.LogInformation($"[{DateTime.UtcNow}] Caching request | From cache value - cache-key: {cacheableRequest.CacheKey}");

                return response;
            }

            response = await next();

            if (response is Result result && result.IsSuccess)
            {
                DistributedCacheEntryOptions defaultOptions = new DistributedCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5)
                };

                logger.LogInformation($"[{DateTime.UtcNow}] Caching request | From persistence value - Saved cache-key: {cacheableRequest.CacheKey}");
                var json = JsonSerializer.Serialize(response);
                await cache.SetStringAsync($"-{cacheableRequest.CacheKey}", json, customOptions ?? defaultOptions, cancellationToken);
            }
            return response;
        }


    }
}
