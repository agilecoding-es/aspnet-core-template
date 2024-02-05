using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Template.Domain.DomainEvents.Abstractions;

namespace Template.Domain.Entities.Abastractions
{
    public interface IEntity
    {
        ReadOnlyCollection<IDomainEvent> DomainEvents { get; }

        void AddDomainEvent(IDomainEvent domainEvent);

        void ClearDomainEvents();
    }
}
