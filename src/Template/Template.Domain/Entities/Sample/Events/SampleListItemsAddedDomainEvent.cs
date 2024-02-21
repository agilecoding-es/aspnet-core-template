using Template.Domain.Entities.Identity;
using Template.Domain.Events.Abstractions;

namespace Template.Domain.Entities.Sample.Events
{
    public class SampleListItemsAddedDomainEvent : IDomainEvent
    {
        public SampleListItemsAddedDomainEvent(User user, int id, List<SampleItem> newItems)
        {
            User = user;
            ListId = id;
            NewItems = newItems;
        }

        public User User { get; }
        public int ListId { get; }
        public List<SampleItem> NewItems { get; }
    }
}
