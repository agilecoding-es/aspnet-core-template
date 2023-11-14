using Microsoft.AspNetCore.Identity.UI.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace Template.Infrastructure.Mails
{
    public interface IEmailClient : IEmailSender
    {
        Task SendEmailAsync(string email, string subject, string plainTextMessage, string htmlMessage = null, CancellationToken cancellationToken = default);
        
        Task SendEmailAsync(string email, string subject, string htmlMessage);
    }
}
