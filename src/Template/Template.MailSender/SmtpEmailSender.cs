using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Options;
using System.Net.Mail;
using Template.Configuration;

namespace Template.MailSender
{
    public class SmtpEmailSender : IEmailSender
    {
        protected readonly ISmtpClientWrapper smtpClient;
        protected readonly Mailsettings mailSettings;

        // Get our parameterized configuration
        public SmtpEmailSender(ISmtpClientWrapper smtpClient, IOptions<Mailsettings> mailSettings)
        {
            this.smtpClient = smtpClient ?? throw new ArgumentNullException(nameof(smtpClient));
            this.mailSettings = mailSettings.Value;
        }

        // Use our configuration to send the email by using SmtpClient
        public async Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            try
            {
                await smtpClient.SendMailAsync(
                    new MailMessage(mailSettings.FromEmail, email, subject, htmlMessage) { IsBodyHtml = true }
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
