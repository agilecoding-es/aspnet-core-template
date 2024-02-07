using Microsoft.AspNetCore.Identity.UI.Services;

namespace Template.Infrastructure.EmailService
{
    public interface IEmailClient
    {
        Task SendEmailAsync(string subject, string email, string displayName = null, string plainTextMessage = null, string htmlMessage = null, CancellationToken cancellationToken = default);
    }
}
