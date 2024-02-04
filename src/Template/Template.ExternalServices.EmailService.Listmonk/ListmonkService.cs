using Microsoft.Extensions.Logging;
using System.Text;
using System.Text.Json;
using System.Xml.Linq;
using Template.Configuration;
using Template.ExternalServices.EmailService.DTOs;

namespace Template.ExternalServices.EmailService.Listmonk
{
    public class ListmonkService : IEmailService
    {
        private readonly MailSettingsOptions mailSettings;
        private readonly IHttpClientFactory httpClientFactory;
        private readonly ILogger logger;

        public ListmonkService(MailSettingsOptions mailSettings, IHttpClientFactory httpClientFactory, ILogger<ListmonkService> logger)
        {
            this.mailSettings = mailSettings ?? throw new ArgumentNullException(nameof(mailSettings));
            this.httpClientFactory = httpClientFactory ?? throw new ArgumentNullException(nameof(httpClientFactory));
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<List<EmailTemplateDto>> GetTemplatesAsync()
        {
            try
            {
                var uri = EndPoints.GetTemplates();

                using (var httpClient = httpClientFactory.CreateClient(typeof(ListmonkService).FullName))
                {
                    var response = await httpClient.GetAsync(uri);

                    response.EnsureSuccessStatusCode();

                    var content = await response.Content.ReadAsStringAsync();
                    var result = JsonSerializer.Deserialize<List<EmailTemplateDto>>(content);

                    return result;
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"Error getting all email templates | Exception Message: {ex.Message}");
            }

            return await Task.FromResult(new List<EmailTemplateDto>());
        }

        public async Task<EmailTemplateDto> GetTemplateAsync(int templateId)
        {
            try
            {
                var uri = EndPoints.GetTemplate(templateId);

                using (var httpClient = httpClientFactory.CreateClient(typeof(ListmonkService).FullName))
                {
                    var response = await httpClient.GetAsync(uri);

                    response.EnsureSuccessStatusCode();

                    var content = await response.Content.ReadAsStringAsync();
                    
                    JsonDocument jsonDocument = JsonDocument.Parse(content);
                    JsonElement root = jsonDocument.RootElement.GetProperty("data");
                    string json = root.GetRawText();

                    var result = JsonSerializer.Deserialize<EmailTemplateDto>(json);

                    return result;
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"Error getting email template {templateId.ToString()} | Exception Message: {ex.Message}");
            }

            return await Task.FromResult(new EmailTemplateDto());
        }

        public async Task<string> PreviewemplateAsync(int templateId)
        {
            try
            {
                var uri = EndPoints.PreviewTemplate(templateId);

                using (var httpClient = httpClientFactory.CreateClient(typeof(ListmonkService).FullName))
                {
                    var response = await httpClient.GetAsync(uri);

                    response.EnsureSuccessStatusCode();

                    var content = await response.Content.ReadAsStringAsync();
                    var result = JsonSerializer.Deserialize<string>(content);

                    return result;
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"Error getting email template {templateId.ToString()} | Exception Message: {ex.Message}");
            }

            return await Task.FromResult(string.Empty);
        }

        public async Task<bool> SendEmailAsync<TData>(int templateId, string recipient, TData data)
        {
            try
            {
                var json = JsonSerializer.Serialize(new TransactionalMessageDto<TData>
                {
                    FromEmail = mailSettings.FromEmail,
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
