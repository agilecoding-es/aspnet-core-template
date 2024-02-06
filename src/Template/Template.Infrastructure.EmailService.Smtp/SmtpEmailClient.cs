using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Net;
using System.Net.Mail;
using Template.Infrastructure.EmailService.Smtp.Settings;

namespace Template.Infrastructure.EmailService.Smtp
{
    public class SmtpEmailClient : IEmailClient
    {
        protected readonly MailSettingOptions mailSettings;
        private readonly IWebHostEnvironment environment;
        private readonly ILogger logger;

        // Get our parameterized configuration
        public SmtpEmailClient(IOptions<MailSettingOptions> mailSettings, IWebHostEnvironment environment, ILogger<SmtpEmailClient> logger)
        {
            this.mailSettings = mailSettings?.Value ?? throw new ArgumentNullException(nameof(mailSettings));
            this.environment = environment ?? throw new ArgumentNullException(nameof(environment));
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        // Use our configuration to send the email by using SmtpClient
        public async Task SendEmailAsync(string subject, string email, string displayName = null, string plainTextMessage=null, string htmlMessage = null, CancellationToken cancellationToken = default)
        {
            try
            {
                logger.LogInformation($"Mail Host: {mailSettings.Host}");
                SmtpClient smtpClient = null;
                if (environment.IsDevelopment())
                {
                    smtpClient = new SmtpClient(mailSettings.Host, mailSettings.Port)
                    {
                        EnableSsl = mailSettings.EnableSSL
                    };
                }
                else
                {
                    smtpClient = new SmtpClient(mailSettings.Host, mailSettings.Port)
                    {
                        EnableSsl = mailSettings.EnableSSL,
                        UseDefaultCredentials = false,
                        Credentials = new NetworkCredential(mailSettings.UserName, mailSettings.Password)
                    };
                }
                logger.LogInformation($"Mail Username: {mailSettings.UserName}");
                var emailMessage = new MailMessage(
                    new MailAddress(mailSettings.FromEmail, mailSettings.DisplayName),
                    new MailAddress(email, displayName)
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
            catch (Exception ex)
            {
                //TODO: Agregar Logging
                throw;
            }
        }
    }
}
