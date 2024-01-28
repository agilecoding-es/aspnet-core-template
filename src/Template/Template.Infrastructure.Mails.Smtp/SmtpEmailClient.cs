using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Net;
using System.Net.Mail;
using Template.Configuration;

namespace Template.Infrastructure.Mails.Smtp
{
    public class SmtpEmailClient : IEmailClient
    {
        protected readonly IntegratedMailOptions mailSettings;
        private readonly ILogger logger;

        // Get our parameterized configuration
        public SmtpEmailClient(IOptions<IntegratedMailOptions> mailSettings, ILogger<SmtpEmailClient> logger)
        {
            this.mailSettings = mailSettings.Value;
            this.logger = logger;
        }

        // Use our configuration to send the email by using SmtpClient
        public async Task SendEmailAsync(string email, string subject, string plainTextMessage, string htmlMessage = null, CancellationToken cancellationToken = default)
        {
            try
            {
                logger.LogInformation($"Mail Host: {mailSettings.Host}");
                var smtpClient = new SmtpClient(mailSettings.Host, mailSettings.Port)
                {
                    EnableSsl = mailSettings.EnableSSL,
                    UseDefaultCredentials = false,
                    Credentials = new NetworkCredential(mailSettings.UserName, mailSettings.Password)
                };
                logger.LogInformation($"Mail Username: {mailSettings.UserName}");
                var emailMessage = new MailMessage(
                    new MailAddress(mailSettings.FromEmail, mailSettings.DisplayName),
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
    }
}
