using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using Template.Configuration;

namespace Template.MailSender
{
    internal class AzureEmailSender : IEmailService
    {
        protected readonly ISmtpClientWrapper _smtpClient;
        protected readonly AppSettings appSettings;

        // Get our parameterized configuration
        public AzureEmailSender(ISmtpClientWrapper smtpClient, AppSettings appSettings)
        {
            _smtpClient = smtpClient ?? throw new ArgumentNullException(nameof(smtpClient));
            _mailSettings = appSettings;
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
