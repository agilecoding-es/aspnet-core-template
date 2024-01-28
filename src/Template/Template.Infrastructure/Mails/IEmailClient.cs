using Microsoft.AspNetCore.Identity.UI.Services;

namespace Template.Infrastructure.Mails
{
    public interface IEmailClient
    {
        Task SendEmailAsync(string email, string subject, string plainTextMessage, string htmlMessage = null, CancellationToken cancellationToken = default);
    }
}
