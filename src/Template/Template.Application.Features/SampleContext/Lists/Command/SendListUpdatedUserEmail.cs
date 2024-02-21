using MediatR;
using Template.Domain.Entities.Sample;
using Template.Domain.Entities.Shared;
using Template.ExternalServices.EmailService;
using System.Text.Json.Serialization;
using Template.Application.Features.IdentityContext.Services;
using Microsoft.Extensions.Logging;
using Template.Domain.Entities.Identity;

namespace Template.Application.Features.SampleContext.Lists.Command
{
    public static class SendListUpdatedUserEmail
    {
        public sealed record Command(string UserId, int ListId, string OldName, string NewName) : IRequest<Result>;

        public class Handler : IRequestHandler<Command, Result>
        {
            //TODO: Eliminar dependencia con listmonk
            private readonly UserManager userManager;
            private readonly IEmailService mailService;
            private readonly ILogger<Handler> logger;

            public Handler(UserManager userManager, IEmailService mailService, ILogger<Handler> logger)
            {
                this.userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
                this.mailService = mailService ?? throw new ArgumentNullException(nameof(mailService));
                this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
            }

            public async Task<Result> Handle(Command request, CancellationToken cancellationToken)
            {
                User user = null;
                try
                {
                    user = await userManager.FindByIdAsync(request.UserId);
                    var data = new SendListUpdatedUserEmailDto
                    {
                        ListOwner = user.UserName,
                        OldName = request.OldName,
                        NewName = request.NewName
                    };

                    logger.LogInformation($"Sending email | to UserId: {user.Id} - Email: {SampleMailTemplate.SampleListNameUpdated.ToString()}");
                    await mailService.SendEmailAsync((int)SampleMailTemplate.SampleListNameUpdated, user.Email, data);
                    logger.LogInformation($"Email sent | to UserId: {user.Id} - Email: {SampleMailTemplate.SampleListNameUpdated.ToString()}");

                    return Result.Success();
                }
                catch (Exception ex)
                {
                    logger.LogError($"Error while sending email | to UserId: {user?.Id} - Email: {SampleMailTemplate.SampleListNameUpdated.ToString()}");
                    return Result<int>.Failure(ex);
                }
            }
        }

        private class SendListUpdatedUserEmailDto
        {
            [JsonPropertyName("list_owner")]
            public string ListOwner { get; set; }

            [JsonPropertyName("old_name")]
            public string OldName { get; set; }

            [JsonPropertyName("new_name")]
            public string NewName { get; set; }
        }
    }
}
