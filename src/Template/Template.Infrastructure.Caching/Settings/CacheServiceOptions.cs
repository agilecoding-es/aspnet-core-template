namespace Template.Infrastructure.Caching.Settings
{
    public class CacheServiceOptions
    {
        public const string Key = "CacheService";

        public bool Enabled { get; set; }
        public int? ExpirationScanFrequencyInMinutes { get; set; }
        public long? SizeLimit { get; set; }

    }
}
