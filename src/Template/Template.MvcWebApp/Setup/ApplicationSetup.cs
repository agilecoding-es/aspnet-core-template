using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authentication.MicrosoftAccount;
using Microsoft.AspNetCore.Identity;
using Template.Configuration;

namespace Template.MvcWebApp.Setup
{
    public static class ApplicationSetup
    {
        public static ApplicationLayerBuilder AddApplication(this IServiceCollection services, IConfiguration configuration,Action<IServiceCollection, IConfiguration> builder)
        {
            builder.Invoke(services, configuration);

            return new ApplicationLayerBuilder(services, configuration);
        }

    }
}
