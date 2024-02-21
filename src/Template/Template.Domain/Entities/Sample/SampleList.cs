using System.Collections.ObjectModel;
using System.ComponentModel;
using Template.Common.Assertions;
using Template.Domain.Entities.Abastractions;
using Template.Domain.Entities.Extensions;
using Template.Domain.Entities.Identity;
using Template.Domain.Entities.Sample.Events;
using Template.Domain.Errors;
using Template.Domain.Exceptions;
using static Template.Domain.Errors.DomainErrors;

namespace Template.Domain.Entities.Sample
{
    [DisplayName("List")]
    public class SampleList : Entity<int>, ISoftDelete
    {
        private readonly List<SampleItem> items = new();
        public SampleList() : base()
        {
        }

        protected SampleList(User user, string name)
        {
            User = user;
            Name = name;
         
            AddDomainEvent(new SampleListCreatedDomainEvent(User, Name));
        }

        public string Name { get; private set; }

        public IEnumerable<SampleItem> Items => items.AsReadOnly();

        public string UserId { get; private set; }

        public User User { get; private set; }

        public bool IsDeleted { get; private set; }

        public static SampleList Create(User user, string name) => new SampleList(user,name);

        public void Delete()
        {
            IsDeleted = true;

            AddDomainEvent(new SampleListDeletedDomainEvent(User, Id));
        }

        public void UpdateName(string newName)
        {
            var oldName = Name;
            Name = newName;

            AddDomainEvent(new SampleListNameUpdatedDomainEvent(UserId, Id, oldName, newName));
        }

        public void Add(SampleItem item)
        {
            items.Add(item);

            AddDomainEvent(new SampleListItemAddedDomainEvent(User, Id, item));
        }

        public void AddRange(List<SampleItem> newItems)
        {
            items.AddRange(newItems);

            AddDomainEvent(new SampleListItemsAddedDomainEvent(User, Id, newItems));
        }

        public void Remove(SampleItem item)
        {
            items.Remove(item);

            AddDomainEvent(new SampleListItemRemovedDomainEvent(User, Id, item));
        }

        public void Clear()
        {
            items.Clear();

            AddDomainEvent(new SampleListItemsClearedDomainEvent(User, Id));
        }

        public bool Contains(SampleItem item)
        {
            return Items.Contains(item);
        }

        public void UpdateItems(List<SampleItem> items)
        {
            //TODO: Traducir error
            Assertion<DomainException>.This(items).IsNotNull("Cannot update Items with a null List of Items");

            this.items.RecreateList<int, SampleItem>(items);

            this.items.UpdateList<int, SampleItem>(items);

        }

    }
}
