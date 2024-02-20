namespace Template.Infrastructure.Caching.Redis.Settings
{
    public class RedisCacheServiceOptions 
    {
        public const string Key = "RedisCacheService";

        public bool Enabled { get; set; }
        public string ConnectionString { get; set; }
        public string InstanceName { get; set; }
        public int ConnectRetry { get; set; }
        public int AbsoluteExpirationInMinutes { get; set; }


    }
}
