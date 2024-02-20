using Azure;
using Azure.Communication.Email;
using Microsoft.Extensions.Logging;
using Template.Configuration;
using Template.Infrastructure.EmailService.AzureCommunicationService.Settings;

namespace Template.Infrastructure.EmailService.AzureCommunicationService
{
    public class AzureEmailClient : IEmailClient
    {
        protected readonly AzureEmailServiceOptions emailServiceOptions;
        private readonly ILogger<AzureEmailClient> logger;

        public AzureEmailClient(AzureEmailServiceOptions emailServiceOptions, ILogger<AzureEmailClient> logger)
        {
            this.emailServiceOptions = emailServiceOptions ?? throw new ArgumentNullException(nameof(emailServiceOptions));
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task SendEmailAsync(string subject, string email, string displayName = null, string plainTextMessage = null, string htmlMessage = null, CancellationToken cancellationToken = default)
        {
            try
            {
                var emailClient = new EmailClient(emailServiceOptions.AzureCommunicationServiceConnection);
                EmailContent emailContent = new EmailContent(subject);
                emailContent.PlainText = plainTextMessage;
                emailContent.Html = htmlMessage;

                List<EmailAddress> emailAddresses = new List<EmailAddress>() { new EmailAddress(email, displayName) };
                EmailRecipients emailRecipients = new EmailRecipients(emailAddresses);
                EmailMessage emailMessage = new EmailMessage(emailServiceOptions.FromEmail, emailRecipients, emailContent);

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
