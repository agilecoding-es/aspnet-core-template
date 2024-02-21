using Template.Domain.Entities.Identity;
using Template.Domain.Events.Abstractions;

namespace Template.Domain.Entities.Sample.Events
{
    public class SampleListItemsClearedDomainEvent : IDomainEvent
    {
        public SampleListItemsClearedDomainEvent(User user, int id)
        {
            User = user;
            ListId = id;
        }

        public User User { get; }
        public int ListId { get; }
    }
}
