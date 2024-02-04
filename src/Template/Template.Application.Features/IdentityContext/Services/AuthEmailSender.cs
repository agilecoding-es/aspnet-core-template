using Microsoft.AspNetCore.Identity.UI.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Template.Infrastructure.EmailService;

namespace Template.Application.Features.IdentityContext.Services
{
    public class AuthEmailSender : IEmailSender
    {
        private readonly IEmailClient emailClient;

        public AuthEmailSender(IEmailClient emailClient)
        {
            this.emailClient = emailClient ?? throw new ArgumentNullException(nameof(emailClient));
        }

        public Task SendEmailAsync(string email, string subject, string htmlMessage) => emailClient.SendEmailAsync(subject,email,htmlMessage: htmlMessage);
    }
}
