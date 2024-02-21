using Template.Domain.Entities.Identity;
using Template.Domain.Events.Abstractions;

namespace Template.Domain.Entities.Sample.Events
{
    public class SampleListDeletedDomainEvent : IDomainEvent
    {
        public SampleListDeletedDomainEvent(User user, int id)
        {
            User = user;
            Id = id;
        }

        public User User { get; }
        public int Id { get; }
    }
}
