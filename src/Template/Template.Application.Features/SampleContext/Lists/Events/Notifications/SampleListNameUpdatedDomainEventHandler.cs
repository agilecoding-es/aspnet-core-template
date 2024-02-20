using MassTransit;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Template.Domain.DomainEvents.Sample;

namespace Template.Application.Features.SampleContext.Lists.Events.Notifications
{
    public class SampleListNameUpdatedDomainEventHandler : INotificationHandler<SampleListNameUpdatedDomainEvent>
    {
        private readonly IPublishEndpoint bus;

        public SampleListNameUpdatedDomainEventHandler(IPublishEndpoint bus)
        {
            this.bus = bus ?? throw new ArgumentNullException(nameof(bus));
        }

        public async Task Handle(SampleListNameUpdatedDomainEvent notification, CancellationToken cancellationToken)
        {
            await bus.Publish(notification, cancellationToken);
        }
    }
}
