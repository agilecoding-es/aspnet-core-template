using Microsoft.AspNetCore.Identity.UI.Services;
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
        public async Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            try
            {
                var smtpClient = new SmtpClient();
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
