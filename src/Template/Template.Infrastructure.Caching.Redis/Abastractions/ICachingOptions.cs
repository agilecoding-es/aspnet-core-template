using Microsoft.Extensions.Caching.Distributed;

namespace Template.Infrastructure.Caching.Redis.Abastractions
{
    public interface ICachingOptions
    {
        DistributedCacheEntryOptions Options { get; }
    }
}
