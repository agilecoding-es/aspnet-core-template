using Template.Application.Contracts.EventBus;

namespace Template.Infrastructure.MessageBroker
{
    public sealed class EventBus : IEventBus
    {
        public Task PublishAsync<T>(T message, CancellationToken cancellationToken = default)
        {
        }
    }
}
