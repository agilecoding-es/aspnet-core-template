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
                    .Configure<RedisCacheServiceOptions>(options =>
                    {
                        appBuilder.Configuration.GetSection(RedisCacheServiceOptions.Key).Bind(options);
                    });

            var redisServiceOptions = appBuilder.Configuration.GetSection(RedisCacheServiceOptions.Key).Get<RedisCacheServiceOptions>();

            appBuilder.Services.AddSingleton(redisServiceOptions);

            appBuilder.Services.AddTransient<ICacheService, RedisCacheService>();
            appBuilder.Services.AddTransient<IRedisCacheService, RedisCacheService>();

            var config = ConfigurationOptions.Parse(redisServiceOptions.ConnectionString);
            config.ConnectRetry = redisServiceOptions.ConnectRetry;
            config.AbortOnConnectFail = false;

            appBuilder.Services.AddStackExchangeRedisCache(options =>
            {
                options.Configuration = redisServiceOptions.ConnectionString;
                options.InstanceName = redisServiceOptions.InstanceName;
                options.ConfigurationOptions = config;

            });

            return appBuilder;
        }
    }
}
