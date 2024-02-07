using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;

namespace Template.Infrastructure.Caching.Redis.Service
{
    public class RedisCacheService : IRedisCacheService
    {
        private readonly IDistributedCache distributedCache;

        public RedisCacheService(IDistributedCache distributedCache)
        {
            this.distributedCache = distributedCache ?? throw new ArgumentNullException(nameof(distributedCache));
        }

        public async Task<T> GetAsync<T>(string key, CancellationToken token = default)
        {
            var value = await distributedCache.GetStringAsync(key, token);

            if (value is null)
                return default;

            return JsonSerializer.Deserialize<T>(value);
        }

        public async Task<T> GetAsync<T>(string key, Func<Task<T>> func, CancellationToken token = default)
        {
            var value = await GetValueAsync(key, func, token);

            if (value is not null)
                await SetAsync(key, value, token);

            return value;
        }

        public async Task<T> GetAsync<T>(string key, Func<Task<T>> func, DistributedCacheEntryOptions options = default, CancellationToken token = default)
        {
            var value = await GetValueAsync(key, func, token);

            if (value is not null)
                await SetAsync(key, value, options, token);

            return value;
        }

        public async Task SetAsync<T>(string key, T value, CancellationToken token = default)
        {
            var json = JsonSerializer.Serialize(value);
            await distributedCache.SetStringAsync(key, json, token);
        }

        public async Task SetAsync<T>(string key, T value, DistributedCacheEntryOptions options = default, CancellationToken token = default)
        {
            var json = JsonSerializer.Serialize(value);
            await distributedCache.SetStringAsync(key, json, options, token);
        }

        public async Task RemoveAsync(string key, CancellationToken token = default)
        {
            await distributedCache.RemoveAsync(key, token);
        }

        private async Task<T> GetValueAsync<T>(string key, Func<Task<T>> func, CancellationToken token = default)
        {
            var value = await GetAsync<T>(key, token);

            if (value is not null)
                return value;

            return await func();
        }
    }
}
