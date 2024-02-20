using System.Collections.ObjectModel;
using Template.Common;
using Template.Common.Assertions;
using Template.Common.Exceptions;
using Template.Common.TypesExtensions;
using Template.Domain.DomainEvents.Abstractions;

namespace Template.Domain.Entities.Abastractions
{
    public abstract class Entity<T> : IEquatable<Entity<T>>, IEntity
        where T : IEquatable<T>
    {
        private readonly List<IDomainEvent> domainEvents = new();

        public Entity()
        {
        }

        public Entity(T id)
        {
            Id = id;
        }

        public T Id { get; protected init; }

        public ReadOnlyCollection<IDomainEvent> DomainEvents => domainEvents.AsReadOnly();

        public static bool operator ==(Entity<T> left, Entity<T> right)
        {
            return left is not null && right is not null && left.Equals(right);
        }

        public static bool operator !=(Entity<T> left, Entity<T> right)
        {
            return !(left == right);
        }

        public override bool Equals(object? obj)
        {
            if (obj == null) return false;

            if (obj.GetType() != GetType()) return false;

            if (obj is not Entity<T> entity) return false;

            return entity.Id.Equals(Id);
        }

        public bool Equals(Entity<T>? other)
        {
            if (other == null) return false;

            if (other.GetType() != GetType()) return false;

            return other.Id.Equals(Id);
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }

        public virtual void Update(Entity<T> newValues) { }

        public void AddDomainEvent(IDomainEvent domainEvent)
        {
            Assertion<InternalException>.This(domainEvent).IsNotNull(Constants.InternalErrors.Entity.DomainEvent_CannotBeNull);
            Assertion<InternalException>.This(domainEvent).IsOfType<IDomainEvent>(Constants.InternalErrors.Entity.DomainEvent_ShouldBeOfType);

            domainEvents.Add(domainEvent);
        }

        public void ClearDomainEvents()
        {
            domainEvents.Clear();
        }
    }

}
