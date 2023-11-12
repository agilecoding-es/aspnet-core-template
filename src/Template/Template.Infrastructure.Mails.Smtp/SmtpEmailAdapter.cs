using Microsoft.Extensions.Options;
using System.Net.Mail;
using Template.Configuration;

namespace Template.Infrastructure.Mails.Smtp
{
    public class SmtpEmailAdapter : IEmailClient
    {
        protected readonly Mailsettings mailSettings;

        // Get our parameterized configuration
        public SmtpEmailAdapter(IOptions<Mailsettings> mailSettings)
        {
            this.mailSettings = mailSettings.Value;
        }

        // Use our configuration to send the email by using SmtpClient
        public async Task SendEmailAsync(string email, string subject, string plainTextMessage, string htmlMessage = null, CancellationToken cancellationToken = default)
        {
            try
            {
                var smtpClient = new SmtpClient();

                var emailMessage = new MailMessage(
                    new MailAddress(mailSettings.FromEmail),
                    new MailAddress(email)
                    );
                emailMessage.Subject = subject;
                emailMessage.Body = string.IsNullOrEmpty(htmlMessage) ? plainTextMessage : htmlMessage;
                emailMessage.IsBodyHtml = !string.IsNullOrEmpty(htmlMessage);

                await smtpClient.SendMailAsync(
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
