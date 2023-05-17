using Microsoft.Extensions.Options;
using Moq;
using System.Net.Mail;
using Template.Configuration;
using Template.MailSender.UnitTests.Stubs;

namespace Template.MailSender.UnitTests
{
    public class EmailSenderTests
    {
        Mock<ISmtpClientWrapper>? _smtpClientMock;
        IOptions<Mailsettings> _mailSettings;

        public EmailSenderTests()
        {
            _smtpClientMock = new Mock<ISmtpClientWrapper>();
            _mailSettings = Options.Create<Mailsettings>(new Mailsettings
            {
                FromEmail = "testuser@example.com",
                Host = "smtp.example.com",
                Port = 587,
                EnableSSL = true,
                UserName = "testuser",
                Password = "password"
            });
        }

        [Fact]
        public void EmailSender_CorrectlyConfigured()
        {
            var emailSender = new EmailSenderStub(_smtpClientMock.Object, _mailSettings);

            Assert.Equal(_mailSettings.Value, emailSender.MailSettings);
            Assert.Equal(_smtpClientMock.Object, emailSender.SmtpClient);
        }

        [Fact]
        public async Task EmailSender_SendEmailAsync_Success()
        {
            var emailSender = new EmailSender(_smtpClientMock.Object, _mailSettings);

            await emailSender.SendEmailAsync("recipient@example.com", "Test Subject", "<h1>Test Body</h1>");

            _smtpClientMock.Verify(x => x.SendMailAsync(
                It.Is<MailMessage>(m =>
                    m.From.Address == _mailSettings.Value.FromEmail &&
                    m.To[0].Address == "recipient@example.com" &&
                    m.Subject == "Test Subject" &&
                    m.Body == "<h1>Test Body</h1>" &&
                    m.IsBodyHtml == true)), Times.Once());
        }

        [Fact]
        public async Task EmailSender_SendEmailAsync_Failure()
        {
            _smtpClientMock.Setup(x => x.SendMailAsync(It.IsAny<MailMessage>())).ThrowsAsync(new SmtpException("Mail send failed."));

            
            var emailSender = new EmailSender(_smtpClientMock.Object, _mailSettings);

            await Assert.ThrowsAsync<SmtpException>(() =>
                emailSender.SendEmailAsync("recipient@example.com", "Test Subject", "<h1>Test Body</h1>"));

            _smtpClientMock.Verify(x => x.SendMailAsync(It.IsAny<MailMessage>()), Times.Once());
        }
    }
}