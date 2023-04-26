using System.Net.Mail;
using Microsoft.Extensions.Options;
using Template.Configuration;
using Template.MvcWebApp.Services;

namespace Template.MailSender.UnitTests.Stubs
{
    internal class EmailSenderStub : EmailSender
    {
        public ISmtpClientWrapper SmtpClient => _smtpClient;
        public Mailsettings MailSettings => _mailSettings;

        public EmailSenderStub(ISmtpClientWrapper smtpClient, IOptions<Mailsettings> mailSettings) : base(smtpClient, mailSettings)
        {
        }
    }
}
