using Microsoft.AspNetCore.StaticFiles;
using Microsoft.Extensions.DependencyInjection;
using Template.Configuration;

namespace Template.UIComponents.Configuration
{
    public static class UIComponentsServiceCollectionExtensions
    {
        public static void AddUIComponents(this IServiceCollection services)
        {
            services.AddSingleton<IContentTypeProvider, FileExtensionContentTypeProvider>();

            services.ConfigureOptions(typeof(UIComponentsConfigureOptions));
        }
    }
}
