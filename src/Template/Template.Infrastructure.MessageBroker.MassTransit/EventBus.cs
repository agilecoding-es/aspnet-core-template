using MassTransit;
using Template.Application.Contracts.EventBus;

namespace Template.Infrastructure.MessageBroker.MassTransit
{
    public class EventBus : IEventBus
    {
        private readonly IPublishEndpoint publishEndpoint;

        public EventBus(IPublishEndpoint publishEndpoint)
        {
            this.publishEndpoint = publishEndpoint ?? throw new ArgumentNullException(nameof(publishEndpoint));
        }

        public Task PublishAsync<T>(T message, CancellationToken cancellationToken = default) where T: class => 
            publishEndpoint.Publish(message, cancellationToken);
    }
}
