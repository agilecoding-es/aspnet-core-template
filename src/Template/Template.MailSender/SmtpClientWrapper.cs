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
        private readonly SmtpClient smtpClient;

        public SmtpClientWrapper(SmtpClient smtpClient)
        {
            this.smtpClient = smtpClient;
        }

        public Task SendMailAsync(MailMessage message) => smtpClient.SendMailAsync(message);

    }
}
