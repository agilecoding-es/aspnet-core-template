using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Text;
using System.Text.Json;

namespace Template.Infrastructure.Caching.Service
{
    public class MemoryCacheService : IMemoryCacheService
    {
        private readonly IMemoryCache memoryCache;

        public MemoryCacheService(IMemoryCache memoryCache)
        {
            this.memoryCache = memoryCache ?? throw new ArgumentNullException(nameof(memoryCache));
        }

        public async Task<T> GetAsync<T>(string key, CancellationToken token = default)
        {
            memoryCache.TryGetValue(key, out string value);

            if (value is null)
                return default;

            return await Task.FromResult(JsonSerializer.Deserialize<T>(value));
        }

        public async Task<T> GetAsync<T>(string key, Func<Task<T>> func, CancellationToken token = default)
        {
            var value = await GetValueAsync(key, func, token);

            if (value is not null)
                await SetAsync(key, value, token);

            return value;
        }

        public async Task<T> GetAsync<T>(string key, Func<Task<T>> func, MemoryCacheEntryOptions options = default, CancellationToken token = default)
        {
            var value = await GetValueAsync(key, func, token);

            if (value is not null)
                await SetAsync(key, value, options, token);

            return value;
        }

        public async Task SetAsync<T>(string key, T value, CancellationToken token = default)
        {
            using (MemoryStream stream = new MemoryStream())
            {
                // Serializar el objeto al stream de manera asíncrona
                await JsonSerializer.SerializeAsync(stream, value, cancellationToken: token);

                // Obtener la cadena JSON del stream
                string json = Encoding.UTF8.GetString(stream.ToArray());

                // Almacenar la cadena JSON en MemoryCache
                memoryCache.Set(key, json);
            }
        }

        public async Task SetAsync<T>(string key, T value, MemoryCacheEntryOptions options = default, CancellationToken token = default)
        {
            using (MemoryStream stream = new MemoryStream())
            {
                // Serializar el objeto al stream de manera asíncrona
                await JsonSerializer.SerializeAsync(stream, value, cancellationToken: token);

                // Obtener la cadena JSON del stream
                string json = Encoding.UTF8.GetString(stream.ToArray());

                // Almacenar la cadena JSON en MemoryCache
                memoryCache.Set(key, json, options);
            }
        }
        
        public Task RemoveAsync(string key, CancellationToken token = default)
        {
            memoryCache.Remove(key);
            return Task.CompletedTask;
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
