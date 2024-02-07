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
                    .Configure<CacheServiceSettingOptions>(options =>
                    {
                        appBuilder.Configuration.GetSection(CacheServiceSettingOptions.Key).Bind(options);
                    });

            var cacheSettings = appBuilder.Configuration.GetSection(CacheServiceSettingOptions.Key).Get<CacheServiceSettingOptions>();
            appBuilder.Services.AddSingleton(cacheSettings);

            appBuilder.Services.AddTransient<ICacheService, MemoryCacheService>();
            appBuilder.Services.AddTransient<IMemoryCacheService, MemoryCacheService>();

            appBuilder.Services.AddMemoryCache(options =>
            {
                options.ExpirationScanFrequency = TimeSpan.FromMinutes(cacheSettings?.ExpirationScanFrequencyInMinutes ?? 10);
                options.SizeLimit = cacheSettings?.SizeLimit ?? 1024;
            });

            return appBuilder;
        }
    }
}
