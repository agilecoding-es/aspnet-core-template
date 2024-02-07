using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;
using Template.Infrastructure.Caching.Service;

namespace Template.Infrastructure.Caching.Redis.Service
{
    public interface IRedisCacheService : ICacheService
    {
        Task<T> GetAsync<T>(string key, Func<Task<T>> func, DistributedCacheEntryOptions options = default, CancellationToken token = default);
        Task SetAsync<T>(string key, T value, DistributedCacheEntryOptions options = default, CancellationToken token = default);

    }
}
