namespace Template.Infrastructure.Caching.Redis.Abastractions
{
    public interface ICacheable
    {
        string CacheKey { get; }
    }
}
