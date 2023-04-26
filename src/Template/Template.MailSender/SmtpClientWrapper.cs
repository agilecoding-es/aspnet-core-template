using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace Template.MailSender
{
    public class SmtpClientWrapper : ISmtpClientWrapper
    {
        private readonly SmtpClient _smtpClient;

        public SmtpClientWrapper(SmtpClient smtpClient)
        {
            _smtpClient = smtpClient;
        }

        public Task SendMailAsync(MailMessage message) => _smtpClient.SendMailAsync(message);

    }
}
