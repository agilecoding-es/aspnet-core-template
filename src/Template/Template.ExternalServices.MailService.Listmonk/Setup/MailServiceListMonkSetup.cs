using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Net.Http.Headers;
using System.Text;
using Template.ExternalServices.MailService;
using Template.ExternalServices.MailService.Listmonk;

namespace Template.Configuration.Setup
{
    public static class IAppBuilderExtensionsMailServiceListmonkSetup
    {
        public static IAppBuilder AddListmonkMailService(this IAppBuilder appBuilder)
        {
            var settings = appBuilder.Configuration.Get<AppSettings>();
            var listmonkSettings = settings.ExternalServices.MailService;
            
            if (listmonkSettings.Enabled)
            {
                appBuilder.Services.AddScoped<IMailService, ListmonkService>();
                appBuilder.Services.AddHttpClient(typeof(ListmonkService).FullName, client =>
                {
                    client.BaseAddress = new Uri(listmonkSettings.Url);
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    var authBytes = Encoding.ASCII.GetBytes($"{listmonkSettings.User}:{listmonkSettings.Password}");
                    client.DefaultRequestHeaders.Authorization =
                        new AuthenticationHeaderValue("Basic", Convert.ToBase64String(authBytes));

                });
            }

            return appBuilder;
        }
    }
}
