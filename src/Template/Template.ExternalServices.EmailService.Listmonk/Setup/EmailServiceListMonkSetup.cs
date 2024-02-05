using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Net.Http.Headers;
using System.Text;
using Template.ExternalServices.EmailService;
using Template.ExternalServices.EmailService.Listmonk;

namespace Template.Configuration.Setup
{
    public static class IAppBuilderExtensionsEmailServiceListmonkSetup
    {
        public static IAppBuilder AddListmonkEmailService(this IAppBuilder appBuilder)
        {

            appBuilder.Services
                        .Configure<ExternalEmailServiceOptions>(options =>
                        {
                            appBuilder.Configuration.GetSection(ExternalEmailServiceOptions.Key).Bind(options);
                        });
            var externalMailService = appBuilder.Configuration.GetSection(ExternalEmailServiceOptions.Key).Get<ExternalEmailServiceOptions>();
            appBuilder.Services.AddSingleton(externalMailService);

            if (externalMailService.Enabled && externalMailService.ServiceName.ToLower() == "listmonk")
            {
                appBuilder.Services.AddScoped<IEmailService, ListmonkService>();
                appBuilder.Services.AddHttpClient(typeof(ListmonkService).FullName, client =>
                {
                    client.BaseAddress = new Uri(externalMailService.Url);
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    var authBytes = Encoding.ASCII.GetBytes($"{externalMailService.User}:{externalMailService.Password}");
                    client.DefaultRequestHeaders.Authorization =
                        new AuthenticationHeaderValue("Basic", Convert.ToBase64String(authBytes));

                });
            }

            return appBuilder;
        }
    }
}
