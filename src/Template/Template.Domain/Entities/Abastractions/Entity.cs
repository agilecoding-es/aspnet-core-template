using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Template.Domain.Entities.Abastractions
{
    public record Key(Guid Value);

    public abstract class Entity<T> : IEquatable<Entity<T>>
        where T : Key
    {
        public Entity(T id)
        {
            Id = id;
        }

        public T Id { get; private init; }

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

            return entity.Id == Id;
        }

        public bool Equals(Entity<T>? other)
        {
            if (other == null) return false;

            if (other.GetType() != GetType()) return false;

            return other.Id == Id;
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }

        public virtual void Update(Entity<T> newValues) { }
    }

}
