using Template.Domain.Events.Abstractions;

namespace Template.Domain.Entities.Sample.Events
{
    public class SampleListNameUpdatedDomainEvent : IDomainEvent
    {
        public SampleListNameUpdatedDomainEvent(string userId, int listId, string oldName, string newName)
        {
            UserId = userId;
            ListId = listId;
            OldName = oldName;
            NewName = newName;
        }

        public string UserId { get; }
        public int ListId { get; }
        public string OldName { get; }
        public string NewName { get; }
    }
}
