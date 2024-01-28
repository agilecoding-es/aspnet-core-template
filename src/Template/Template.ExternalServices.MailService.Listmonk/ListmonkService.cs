using Microsoft.Extensions.Logging;
using System.Text;
using System.Text.Json;
using System.Xml.Linq;
using Template.ExternalServices.MailService.Listmonk.DTOs;

namespace Template.ExternalServices.MailService.Listmonk
{
    public class ListmonkService : IMailService
    {
        private readonly IHttpClientFactory httpClientFactory;
        private readonly ILogger logger;

        public ListmonkService(IHttpClientFactory httpClientFactory, ILogger<ListmonkService> logger)
        {
            this.httpClientFactory = httpClientFactory ?? throw new ArgumentNullException(nameof(httpClientFactory));
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<bool> SendEmailAsync<TData>(short templateId, string recipient, TData data)
        {
            try
            {
                var json = JsonSerializer.Serialize(new TransactionalMessageDto<TData>
                {
                    SubscriberEmail = recipient,
                    TemplateId = templateId,
                    Data = data,
                    ContentType = "html"
                });

                var uri = EndPoints.Transactional();

                using (var httpClient = httpClientFactory.CreateClient(typeof(ListmonkService).FullName))
                {
                    var content = new StringContent(json, Encoding.UTF8, "application/json");
                    var response = await httpClient.PostAsync(uri, content);

                    return response.IsSuccessStatusCode;
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"Error sending email {templateId.ToString()} | Exception Message: {ex.Message}");
            }
            
            return false;
        }
    }
}
