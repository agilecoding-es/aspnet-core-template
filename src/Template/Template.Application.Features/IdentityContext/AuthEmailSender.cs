using Microsoft.AspNetCore.Identity.UI.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Template.Infrastructure.Mails;

namespace Template.Application.Features.IdentityContext
{
    public class AuthEmailSender : IEmailSender
    {
        private readonly IEmailClient emailClient;

        public AuthEmailSender(IEmailClient emailClient)
        {
            this.emailClient = emailClient ?? throw new ArgumentNullException(nameof(emailClient));
        }

        public Task SendEmailAsync(string email, string subject, string htmlMessage) => emailClient.SendEmailAsync(email, subject, null, htmlMessage);
    }
}
