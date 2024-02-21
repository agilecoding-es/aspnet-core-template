using Template.Domain.Entities.Identity;
using Template.Domain.Events.Abstractions;

namespace Template.Domain.Entities.Sample.Events
{
    public class SampleListItemRemovedDomainEvent : IDomainEvent
    {
        public SampleListItemRemovedDomainEvent(User user, int id, SampleItem item)
        {
            User = user;
            ListId = id;
            Item = item;
        }

        public User User { get; }
        public int ListId { get; }
        public SampleItem Item { get; }
    }
}
