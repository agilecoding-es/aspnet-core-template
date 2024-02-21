using MediatR;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Template.Infrastructure.Caching;
using Template.Infrastructure.Caching.Memory;
using Template.Infrastructure.Caching.Memory.Mediator.Behavior;

namespace Template.Configuration.Setup.Memory
{
    public static class IAppBuilderExtensionsCachingSetup
    {

        public static IAppBuilder AddCacheService(this IAppBuilder appBuilder)
        {
            appBuilder.Services
                    .Configure<MemoryCacheOptions>(options =>
                    {
                        appBuilder.Configuration.GetSection(MemoryCacheConstants.MemoryCacheOptionsKey).Bind(options);
                    });

            var memoryCacheOptions = appBuilder.Configuration.GetSection(MemoryCacheConstants.MemoryCacheOptionsKey).Get<MemoryCacheOptions>();
            appBuilder.Services.AddSingleton(memoryCacheOptions);

            appBuilder.Services.AddMemoryCache(options =>
            {
                options.ExpirationScanFrequency = memoryCacheOptions.ExpirationScanFrequency;
                options.SizeLimit = memoryCacheOptions?.SizeLimit ?? MemoryCacheConstants.DefaultSizeLimit;
            });

            appBuilder.Services.AddTransient<ICacheService, CacheService>();
            appBuilder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(MemoryCachingBehavior<,>));

            return appBuilder;
        }
    }
}
