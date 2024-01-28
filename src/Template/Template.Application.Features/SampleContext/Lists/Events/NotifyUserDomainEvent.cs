using MediatR;
using Template.Application.Features.SampleContext.Lists.Command;
using Template.Domain.DomainEvents.Sample;

namespace Template.Application.Features.SampleContext.Lists.Events
{
    public class NotifyUserDomainEvent : INotificationHandler<SampleListNameUpdatedDomainEvent>
    {
        private readonly IMediator mediator;

        public NotifyUserDomainEvent(IMediator mediator)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task Handle(SampleListNameUpdatedDomainEvent notification, CancellationToken cancellationToken)
        {
            await mediator.Send(new SendListUpdatedUserMail.Command(notification.UserId, notification.ListId, notification.OldName, notification.NewName), cancellationToken);
        }
    }
}
