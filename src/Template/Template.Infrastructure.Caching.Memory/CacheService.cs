using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Template.Infrastructure.Caching.Memory
{
    public class CacheService:ICacheService
    {
        private readonly IMemoryCache memoryCache;
        private readonly ILogger<CacheService> logger;

        public CacheService(IMemoryCache memoryCache, ILogger<CacheService> logger)
        {
            this.memoryCache = memoryCache ?? throw new ArgumentNullException(nameof(memoryCache));
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public Task RemoveAsync(string key)
        {
            try
            {
                memoryCache.Remove($"-{key}");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"Error removing cache-key: {key} | {ex.Message}");
            }
            return Task.CompletedTask;
        }
    }
}
