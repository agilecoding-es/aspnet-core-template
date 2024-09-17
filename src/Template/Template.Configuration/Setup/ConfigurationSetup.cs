using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Template.Configuration.Setup
{
    public static class ConfigurationSetup
    {
        public static IAppBuilder ConfigureSettings(this IAppBuilder builder)
        {
            builder.Services
                        .Configure<AppSettings>(builder.Configuration)
                        .Configure<LoggingExceptionsOptions>(options =>
                        {
                            builder.Configuration.GetSection(LoggingExceptionsOptions.Key).Bind(options);
                        });


            builder.Services.AddSingleton(builder.Configuration.Get<AppSettings>());
            builder.Services.AddSingleton(builder.Configuration.GetSection(LoggingExceptionsOptions.Key).Get<LoggingExceptionsOptions>());

            return builder;
        }

        public static T ConfigureSetting<T>(this IAppBuilder builder, string key) where T : class
        {
            builder.Services.Configure<T>( options =>
            {
                builder.Configuration.GetSection(key).Bind(options);
            });

            var instance = builder.Configuration.Get<T>();
            builder.Services.AddSingleton(typeof(T), instance);

            return instance;
        }
    }
}
