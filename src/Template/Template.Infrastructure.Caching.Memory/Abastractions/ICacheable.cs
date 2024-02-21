namespace Template.Infrastructure.Caching.Memory.Abastractions
{
    public interface ICacheable
    {
        string CacheKey { get; }
    }
}
