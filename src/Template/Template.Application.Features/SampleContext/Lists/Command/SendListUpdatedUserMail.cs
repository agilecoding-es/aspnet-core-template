using MediatR;
using Template.Application.Features.IdentityContext;
using Template.Domain.Entities.Sample;
using Template.Domain.Entities.Shared;
using Template.ExternalServices.MailService;
using System.Text.Json.Serialization;

namespace Template.Application.Features.SampleContext.Lists.Command
{
    public static class SendListUpdatedUserMail
    {
        public sealed record Command(string UserId, int ListId, string OldName, string NewName) : IRequest<Result>;

        public class Handler : IRequestHandler<Command, Result>
        {
            //TODO: Eliminar dependencia con listmonk
            private readonly UserManager userManager;
            private readonly IMailService mailService;

            public Handler(UserManager userManager, IMailService mailService)
            {
                this.userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
                this.mailService = mailService ?? throw new ArgumentNullException(nameof(mailService));
            }

            public async Task<Result> Handle(Command request, CancellationToken cancellationToken)
            {
                try
                {
                    var user = await userManager.FindByIdAsync(request.UserId);
                    var data = new SendListUpdatedUserMailDto
                    {
                        ListOwner = user.UserName,
                        OldName = request.OldName,
                        NewName = request.NewName
                    };

                    await mailService.SendEmailAsync((short)SampleMailTemplate.SampleListNameUpdated, user.Email, data);

                    return Result.Success();
                }
                catch (Exception ex)
                {
                    return Result<int>.Failure(ex);
                }
            }
        }

        private class SendListUpdatedUserMailDto
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
