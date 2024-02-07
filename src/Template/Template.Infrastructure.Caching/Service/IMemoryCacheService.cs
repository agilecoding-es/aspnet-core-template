using Microsoft.Extensions.Caching.Memory;

namespace Template.Infrastructure.Caching.Service
{
    public interface IMemoryCacheService : ICacheService
    {
        Task<T> GetAsync<T>(string key, Func<Task<T>> func, MemoryCacheEntryOptions options = default, CancellationToken token = default);
        Task SetAsync<T>(string key, T value, MemoryCacheEntryOptions options = default, CancellationToken token = default);

    }
}
