using MassTransit;
using MediatR;
using Microsoft.Extensions.Logging;
using Template.Application.Features.SampleContext.Lists.Command;
using Template.Domain.DomainEvents.Sample;

namespace Template.Application.Features.SampleContext.Lists.Events.MessageBroker
{
    public class NotifyUserDomainEventConsumer : IConsumer<SampleListNameUpdatedDomainEvent>
    {
        private readonly IMediator mediator;
        private readonly ILogger<NotifyUserDomainEventConsumer> logger;

        public NotifyUserDomainEventConsumer(IMediator mediator, ILogger<NotifyUserDomainEventConsumer> logger)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task Consume(ConsumeContext<SampleListNameUpdatedDomainEvent> context)
        {
            await mediator.Send(new SendListUpdatedUserEmail.Command(context.Message.UserId, context.Message.ListId, context.Message.OldName, context.Message.NewName), CancellationToken.None);
        }
    }
}
