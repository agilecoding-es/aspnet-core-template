using Template.Domain.Entities.Identity;
using Template.Domain.Events.Abstractions;

namespace Template.Domain.Entities.Sample.Events
{
    public class SampleListCreatedDomainEvent : IDomainEvent
    {
        public SampleListCreatedDomainEvent(User user, string name)
        {
            User = user;
            Name = name;
        }

        public User User { get; }
        public string Name { get; }
    }
}
