using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Template.Domain.DomainEvents.Abstractions;

namespace Template.Domain.DomainEvents.Sample
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
