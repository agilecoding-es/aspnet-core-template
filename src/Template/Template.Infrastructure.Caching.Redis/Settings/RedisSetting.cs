using Template.Infrastructure.Caching.Settings;

namespace Template.Infrastructure.Caching.Redis.Settings
{
    public class RedisServiceSettingOptions 
    {
        public const string Key = "RedisSettings";

        public bool Enabled { get; set; }
        public string ConnectionString { get; set; }
        public string InstanceName { get; set; }
        public int ConnectRetry { get; set; }
        public int AbsoluteExpirationInMinutes { get; set; }


    }
}
