using Microsoft.Extensions.Options;
using Template.Configuration;

namespace Template.MailSender.UnitTests.Stubs
{
    internal class EmailSenderStub : SmtpEmailSender
    {
        public ISmtpClientWrapper SmtpClient => smtpClient;
        public Mailsettings MailSettings => mailSettings;

        public EmailSenderStub(ISmtpClientWrapper smtpClient, IOptions<Mailsettings> mailSettings) : base(smtpClient, mailSettings)
        {
        }
    }
}
