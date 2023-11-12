using Azure;
using Azure.Communication.Email;
using Template.Configuration;

namespace Template.Infrastructure.Mails.AzureCommunicationService
{
    public class AzureEmailAdapter : IEmailClient
    {
        protected readonly AppSettings appSettings;
        protected readonly Mailsettings mailSettings;

        public AzureEmailAdapter(AppSettings appSettings)
        {
            this.appSettings = appSettings;
            mailSettings = appSettings.Mailsettings;
        }

        public async Task SendEmailAsync(string email, string subject, string plainTextMessage, string htmlMessage = null, CancellationToken cancellationToken = default)
        {
            try
            {
                var emailClient = new EmailClient(appSettings.ConnectionStrings.AzureCommunicationServiceConnection);
                EmailContent emailContent = new EmailContent(subject);
                emailContent.PlainText = plainTextMessage;
                emailContent.Html = htmlMessage;

                List<EmailAddress> emailAddresses = new List<EmailAddress>() { new EmailAddress(email) };
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

        public Task SendEmailAsync(string email, string subject, string htmlMessage) => SendEmailAsync(email, subject, null, htmlMessage);
    }
}
