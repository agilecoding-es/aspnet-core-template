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
        protected readonly EmailServiceOptions emailServiceOptions;
        private readonly IWebHostEnvironment environment;
        private readonly ILogger logger;

        // Get our parameterized configuration
        public SmtpEmailClient(IOptions<EmailServiceOptions> emailServiceOptions, IWebHostEnvironment environment, ILogger<SmtpEmailClient> logger)
        {
            this.emailServiceOptions = emailServiceOptions?.Value ?? throw new ArgumentNullException(nameof(emailServiceOptions));
            this.environment = environment ?? throw new ArgumentNullException(nameof(environment));
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        // Use our configuration to send the email by using SmtpClient
        public async Task SendEmailAsync(string subject, string email, string displayName = null, string plainTextMessage=null, string htmlMessage = null, CancellationToken cancellationToken = default)
        {
            try
            {
                logger.LogInformation($"Mail Host: {emailServiceOptions.Host}");
                SmtpClient smtpClient = null;
                if (environment.IsDevelopment())
                {
                    smtpClient = new SmtpClient(emailServiceOptions.Host, emailServiceOptions.Port)
                    {
                        EnableSsl = emailServiceOptions.EnableSSL
                    };
                }
                else
                {
                    smtpClient = new SmtpClient(emailServiceOptions.Host, emailServiceOptions.Port)
                    {
                        EnableSsl = emailServiceOptions.EnableSSL,
                        UseDefaultCredentials = false,
                        Credentials = new NetworkCredential(emailServiceOptions.UserName, emailServiceOptions.Password)
                    };
                }
                logger.LogInformation($"Mail Username: {emailServiceOptions.UserName}");
                var emailMessage = new MailMessage(
                    new MailAddress(emailServiceOptions.FromEmail, emailServiceOptions.DisplayName),
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
