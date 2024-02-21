
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;

namespace Template.Infrastructure.Caching.Redis
{
    public class CacheService : ICacheService
    {
        private readonly IDistributedCache distributedCache;
        private readonly ILogger<CacheService> logger;

        public CacheService(IDistributedCache distributedCache, ILogger<CacheService> logger)
        {
            this.distributedCache = distributedCache ?? throw new ArgumentNullException(nameof(distributedCache));
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task RemoveAsync(string key)
        {
            try
            {
                await distributedCache.RemoveAsync($"-{key}");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"Error removing cache-key: {key} | {ex.Message}");
            }
        }
    }
}
