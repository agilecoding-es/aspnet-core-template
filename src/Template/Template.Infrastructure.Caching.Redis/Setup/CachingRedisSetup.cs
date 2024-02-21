using MediatR;
using Microsoft.Extensions.Caching.StackExchangeRedis;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;
using Template.Infrastructure.Caching;
using Template.Infrastructure.Caching.Redis;
using Template.Infrastructure.Caching.Redis.Mediator.Behavior;

namespace Template.Configuration.Setup
{
    public static class CachingRedisSetup
    {
        public static IAppBuilder AddRedisCacheService(this IAppBuilder appBuilder)
        {
            appBuilder.Services
                    .Configure<RedisCacheOptions>(options =>
                    {
                        appBuilder.Configuration.GetSection(RedisCacheConstants.RedisCacheOptionsKey).Bind(options);
                    });

            var redisCacheOptions = appBuilder.Configuration.GetSection(RedisCacheConstants.RedisCacheOptionsKey).Get<RedisCacheOptions>();

            appBuilder.Services.AddSingleton(redisCacheOptions);



            var config = ConfigurationOptions.Parse(redisCacheOptions.Configuration);
            config.ConnectRetry = redisCacheOptions.ConfigurationOptions.ConnectRetry;
            config.AbortOnConnectFail = false;

            appBuilder.Services.AddStackExchangeRedisCache(options =>
            {
                options.Configuration = redisCacheOptions.Configuration;
                options.InstanceName = redisCacheOptions.InstanceName;
                options.ConfigurationOptions = config;
            });

            appBuilder.Services.AddTransient<ICacheService, CacheService>();
            appBuilder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(RedisCachingBehavior<,>));

            return appBuilder;
        }
    }
}
