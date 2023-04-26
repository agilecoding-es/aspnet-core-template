using System.Net.Mail;
using System.Net;
using Microsoft.AspNetCore.Identity.UI.Services;
using Template.MailSender;
using Template.Configuration;
using Microsoft.Extensions.Options;

namespace Template.MvcWebApp.Services
{
    public class EmailSender : IEmailSender, IEmailService
    {
        protected readonly ISmtpClientWrapper _smtpClient;
        protected readonly Mailsettings _mailSettings;

        // Get our parameterized configuration
        public EmailSender(ISmtpClientWrapper smtpClient, IOptions<Mailsettings> mailSettings)
        {
            _smtpClient = smtpClient ?? throw new ArgumentNullException(nameof(smtpClient));
            _mailSettings = mailSettings.Value;
        }

        // Use our configuration to send the email by using SmtpClient
        public Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            try
            {
                return _smtpClient.SendMailAsync(
                    new MailMessage(_mailSettings.FromEmail, email, subject, htmlMessage) { IsBodyHtml = true }
                );
            }
            catch
            {
                //TODO: Agregar Logging
                throw;
            }
        }
    }
}
