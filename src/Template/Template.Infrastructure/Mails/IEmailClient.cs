using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace Template.Infrastructure.Mails
{
    public interface IEmailClient
    {
        Task SendEmailAsync(string email, string subject, string htmlMessage);
    }
}
