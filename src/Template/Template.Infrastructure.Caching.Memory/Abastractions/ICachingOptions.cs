using Microsoft.Extensions.Caching.Memory;

namespace Template.Infrastructure.Caching.Memory.Abastractions
{
    public interface ICachingOptions
    {
        MemoryCacheEntryOptions Options { get; }
    }
}
