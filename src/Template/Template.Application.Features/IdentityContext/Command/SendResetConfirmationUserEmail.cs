using MediatR;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Localization;
using System.Text.Encodings.Web;
using System.Text.Json.Serialization;
using Template.Application.Features.IdentityContext.Services;
using Template.Common.Extensions;
using Template.Domain.Entities.Identity;
using Template.Domain.Entities.Sample;
using Template.Domain.Entities.Shared;
using Template.ExternalServices.EmailService;

namespace Template.Application.Features.IdentityContext.Command
{
    public static class SendResetConfirmationUserEmail
    {
        public sealed record Command(string UserId, string email, string callbackUrl) : IRequest<Result>;

        public class Handler : IRequestHandler<Command, Result>
        {
            //TODO: Eliminar dependencia con listmonk
            private readonly UserManager userManager;
            private readonly IEmailSender emailSender;
            private readonly IEmailService mailService;
            private readonly IStringLocalizer localizer;

            public Handler(UserManager userManager, IEmailSender emailSender, IEmailService mailService, IStringLocalizer localizer)
            {
                this.userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
                this.emailSender = emailSender ?? throw new ArgumentNullException(nameof(emailSender));
                this.mailService = mailService ?? throw new ArgumentNullException(nameof(mailService));
                this.localizer = localizer ?? throw new ArgumentNullException(nameof(localizer));
            }

            public async Task<Result> Handle(Command request, CancellationToken cancellationToken)
            {
                try
                {
                    var user = await userManager.FindByIdAsync(request.UserId);
                
                    var emailTemplate = await mailService.GetTemplateAsync((int)IdentityMailTemplate.ResetPasswordConfirmation);
                    emailTemplate.Body = ReplaceBodyTemplate(emailTemplate.Body, user, request.callbackUrl);

                    await emailSender.SendEmailAsync(
                        request.email,
                        emailTemplate.Subject.IsNullOrWhiteSpace() ? localizer.GetString("Identity_Account_ForgotPassword_ConfirmEmailSubject") : emailTemplate.Subject.Trim(),
                        emailTemplate.Body.IsNullOrWhiteSpace() ? localizer.GetString("Identity_Account_ForgotPassword_ConfirmEmailBody", HtmlEncoder.Default.Encode(request.callbackUrl)) : emailTemplate.Body);

                    return Result.Success();
                }
                catch (Exception ex)
                {
                    return Result<int>.Failure(ex);
                }
            }

            private string ReplaceBodyTemplate(string template, User user, string url) =>
                        template.Replace("{{ .Tx.Data.name }}", user.UserName)
                                .Replace("{{ .Tx.Data.reset_url }}", url);
        }
    }
}
