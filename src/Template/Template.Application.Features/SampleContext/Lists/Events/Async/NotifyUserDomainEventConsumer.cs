using MassTransit;
using MediatR;
using Microsoft.Extensions.Logging;
using Template.Application.Features.SampleContext.Lists.Command;
using Template.Domain.Entities.Sample.Events;

namespace Template.Application.Features.SampleContext.Lists.Events.Async
{
    public class NotifyUserDomainEventConsumer : IConsumer<SampleListNameUpdatedDomainEvent>
    {
        private readonly IMediator mediator;

        public NotifyUserDomainEventConsumer(IMediator mediator)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task Consume(ConsumeContext<SampleListNameUpdatedDomainEvent> context)
        {
            await mediator.Send(new SendListUpdatedUserEmail.Command(context.Message.UserId, context.Message.ListId, context.Message.OldName, context.Message.NewName), CancellationToken.None);
        }
    }
}
