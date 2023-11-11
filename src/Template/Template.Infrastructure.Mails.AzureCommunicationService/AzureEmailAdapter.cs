using Azure;
using Azure.Communication.Email;
using Microsoft.AspNetCore.Identity.UI.Services;
using System.Net.Mail;
using Template.Configuration;

namespace Template.Infrastructure.Mails.AzureCommunicationService
{
    public class AzureEmailAdapter : IEmailClient
    {
        protected readonly AppSettings appSettings;

        // Get our parameterized configuration
        public AzureEmailAdapter(AppSettings appSettings)
        {
            this.appSettings = appSettings;
        }

        // Use our configuration to send the email by using SmtpClient
        public async Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            try
            {
                var emailClient = new EmailClient(appSettings.ConnectionStrings.AzureCommunicationServiceConnection);
                EmailContent content;
                var sendOperation = await emailClient.SendAsync(
                    WaitUntil.Completed,
                    appSettings.Mailsettings.FromEmail,
                    email,
                    subject,
                    htmlMessage
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
