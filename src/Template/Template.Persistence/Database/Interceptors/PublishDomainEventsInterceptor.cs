using MassTransit;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Template.Domain.Entities.Abastractions;

namespace Template.Persistence.Database.Interceptors
{
    public class PublishDomainEventsInterceptor : SaveChangesInterceptor
    {
        private readonly IPublisher publisher;
        private readonly IPublishEndpoint bus;

        public PublishDomainEventsInterceptor(IPublisher publisher, IPublishEndpoint bus)
        {
            this.publisher = publisher ?? throw new ArgumentNullException(nameof(publisher));
            this.bus = bus ?? throw new ArgumentNullException(nameof(bus));
        }

        public override async ValueTask<int> SavedChangesAsync(
            SaveChangesCompletedEventData eventData,
            int result,
            CancellationToken cancellationToken = default)
        {
            if (eventData.Context is not null)
            {
                await PublishDomainEventsAsync(eventData.Context, cancellationToken);
            }

            return result;
        }

        private async Task PublishDomainEventsAsync(DbContext context, CancellationToken cancellationToken)
        {
            var domainEvents = context
                .ChangeTracker
                .Entries<IEntity>()
                .Select(entry => entry.Entity)
                .SelectMany(entity =>
                {
                    var domainEvents = entity.DomainEvents.ToList();

                    entity.ClearDomainEvents();

                    return domainEvents;
                })
                .ToList();

            foreach (var domainEvent in domainEvents)
            {
                await publisher.Publish(domainEvent);
            }
        }
    }
}
