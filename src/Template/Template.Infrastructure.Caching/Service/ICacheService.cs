using Microsoft.Extensions.Caching.Distributed;

namespace Template.Infrastructure.Caching.Service
{
    public interface ICacheService
    {
        Task<T> GetAsync<T>(string key, CancellationToken token = default);
        Task<T> GetAsync<T>(string key, Func<Task<T>> func, CancellationToken token = default);
        Task SetAsync<T>(string key, T value, CancellationToken token = default);
        Task RemoveAsync(string key, CancellationToken token = default);
    }
}
