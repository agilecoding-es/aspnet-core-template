using Azure;
using Azure.Communication.Email;
using Template.Configuration;

namespace Template.Infrastructure.EmailService.AzureCommunicationService
{
    public class AzureEmailClient : IEmailClient
    {
        protected readonly AppSettings appSettings;
        protected readonly MailSettingsOptions mailSettings;

        public AzureEmailClient(MailSettingsOptions mailSettings)
        {
            this.mailSettings = mailSettings;
        }

        public async Task SendEmailAsync(string subject, string email, string displayName = null, string plainTextMessage = null, string htmlMessage = null, CancellationToken cancellationToken = default)
        {
            try
            {
                var emailClient = new EmailClient(appSettings.ConnectionStrings.AzureCommunicationServiceConnection);
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
                //TODO: Agregar Logging
                throw;
            }
        }
    }
}
