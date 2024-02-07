using Azure;
using Azure.Communication.Email;
using Microsoft.Extensions.Logging;
using Template.Configuration;
using Template.Infrastructure.EmailService.AzureCommunicationService.Settings;

namespace Template.Infrastructure.EmailService.AzureCommunicationService
{
    public class AzureEmailClient : IEmailClient
    {
        protected readonly AzureMailSettingOptions mailSettings;
        private readonly ILogger<AzureEmailClient> logger;

        public AzureEmailClient(AzureMailSettingOptions mailSettings, ILogger<AzureEmailClient> logger)
        {
            this.mailSettings = mailSettings ?? throw new ArgumentNullException(nameof(mailSettings));
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task SendEmailAsync(string subject, string email, string displayName = null, string plainTextMessage = null, string htmlMessage = null, CancellationToken cancellationToken = default)
        {
            try
            {
                var emailClient = new EmailClient(mailSettings.AzureCommunicationServiceConnection);
                EmailContent emailContent = new EmailContent(subject);
                emailContent.PlainText = plainTextMessage;
                emailContent.Html = htmlMessage;

                List<EmailAddress> emailAddresses = new List<EmailAddress>() { new EmailAddress(email, displayName) };
                EmailRecipients emailRecipients = new EmailRecipients(emailAddresses);
                EmailMessage emailMessage = new EmailMessage(mailSettings.FromEmail, emailRecipients, emailContent);

                var sendOperation = await emailClient.SendAsync(
                    WaitUntil.Completed,
                    emailMessage,
                    cancellationToken
                    );

                return;
            }
            catch
            {
                logger.LogError("An error occurred while sending the email.");
            }
        }
    }
}
