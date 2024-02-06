using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;
using Template.Infrastructure.Caching.Redis.Service;
using Template.Infrastructure.Caching.Redis.Settings;
using Template.Infrastructure.Caching.Service;

namespace Template.Configuration.Setup
{
    public static class CachingRedisSetup
    {
        public static IAppBuilder AddRedisCacheService(this IAppBuilder appBuilder)
        {
            appBuilder.Services
                    .Configure<RedisServiceSettingOptions>(options =>
                    {
                        appBuilder.Configuration.GetSection(RedisServiceSettingOptions.Key).Bind(options);
                    });

            var redisSettings = appBuilder.Configuration.GetSection(RedisServiceSettingOptions.Key).Get<RedisServiceSettingOptions>();

            appBuilder.Services.AddSingleton(redisSettings);

            appBuilder.Services.AddTransient<ICacheService, RedisCacheService>();
            appBuilder.Services.AddTransient<IRedisCacheService, RedisCacheService>();

            var config = ConfigurationOptions.Parse(redisSettings.ConnectionString);
            config.ConnectRetry = redisSettings.ConnectRetry;
            config.AbortOnConnectFail = false;

            appBuilder.Services.AddStackExchangeRedisCache(options =>
            {
                options.Configuration = redisSettings.ConnectionString;
                options.InstanceName = redisSettings.InstanceName;
                options.ConfigurationOptions = config;

            });

            return appBuilder;
        }
    }
}
