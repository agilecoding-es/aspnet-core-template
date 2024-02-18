using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Template.Infrastructure.Caching.Service;
using Template.Infrastructure.Caching.Settings;

namespace Template.Configuration.Setup
{
    public static class IAppBuilderExtensionsCachingSetup
    {
        public static IAppBuilder AddCacheService(this IAppBuilder appBuilder)
        {
            appBuilder.Services
                    .Configure<CacheServiceOptions>(options =>
                    {
                        appBuilder.Configuration.GetSection(CacheServiceOptions.Key).Bind(options);
                    });

            var cacheServiceOptions = appBuilder.Configuration.GetSection(CacheServiceOptions.Key).Get<CacheServiceOptions>();
            appBuilder.Services.AddSingleton(cacheServiceOptions);

            appBuilder.Services.AddTransient<ICacheService, MemoryCacheService>();
            appBuilder.Services.AddTransient<IMemoryCacheService, MemoryCacheService>();

            appBuilder.Services.AddMemoryCache(options =>
            {
                options.ExpirationScanFrequency = TimeSpan.FromMinutes(cacheServiceOptions?.ExpirationScanFrequencyInMinutes ?? 10);
                options.SizeLimit = cacheServiceOptions?.SizeLimit ?? 1024;
            });

            return appBuilder;
        }
    }
}
