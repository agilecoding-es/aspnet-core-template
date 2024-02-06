namespace Template.Infrastructure.Caching.Abastractions
{
    public interface ICacheable
    {
        string CacheKey { get; }
    }
}
