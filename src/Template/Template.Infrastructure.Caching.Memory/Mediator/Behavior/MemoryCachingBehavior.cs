using MediatR;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Template.Domain.Entities.Shared;
using Template.Infrastructure.Caching.Memory.Abastractions;

namespace Template.Infrastructure.Caching.Memory.Mediator.Behavior
{
    public class MemoryCachingBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    {
        private readonly IMemoryCache cache;
        private readonly ILogger logger;

        public MemoryCachingBehavior(IMemoryCache cache, ILogger<MemoryCachingBehavior<TRequest, TResponse>> logger)
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

            MemoryCacheEntryOptions customOptions = null;
            if (request is ICachingOptions cachingOptions)
            {
                customOptions = cachingOptions.Options;
            }

            logger.LogInformation($"[{DateTime.UtcNow}] Caching request | Checking cache - cache-key: {cacheableRequest.CacheKey}");

            cache.TryGetValue(cacheableRequest.CacheKey, out string value);

            TResponse response = default;
            if (!EqualityComparer<TResponse>.Default.Equals(response, default))
            {
                logger.LogInformation($"[{DateTime.UtcNow}] Caching request | From cache value - cache-key: {cacheableRequest.CacheKey}");

                return response;
            }

            response = await next();

            if (response is Result result && result.IsSuccess)
            {
                MemoryCacheEntryOptions defaultOptions = new MemoryCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5)
                };

                logger.LogInformation($"[{DateTime.UtcNow}] Caching request | From persistence value - Saved cache-key: {cacheableRequest.CacheKey}");
                cache.Set($"{cacheableRequest.CacheKey}", response, customOptions ?? defaultOptions);
            }
            return response;
        }
    }
}
