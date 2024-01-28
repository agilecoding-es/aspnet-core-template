using System.ComponentModel;
using Template.Domain.DomainEvents.Sample;
using Template.Domain.Entities.Abastractions;
using Template.Domain.Entities.Extensions;
using Template.Domain.Entities.Identity;
using Template.Domain.Exceptions;

namespace Template.Domain.Entities.Sample
{
    [DisplayName("List")]
    public class SampleList : Entity<int>
    {
        public SampleList() : base()
        {
        }

        public SampleList(int id) : base(id) { }

        public string Name { get; private set; }

        public List<SampleItem> Items { get; private set; } = new List<SampleItem>();

        public string UserId { get; private set; }
        
        public User User { get; private set; }


        public static SampleList Create(User user, string name)
        {
            var sampleList = new SampleList()
            {
                UserId = user.Id,
                Name = name
            };

            return sampleList;
        }

        public void UpdateName(string newName)
        {
            var oldName = Name;
            Name = newName;

            AddDomainEvent(new SampleListNameUpdatedDomainEvent(UserId,Id, oldName, newName));
        }

        public void Add(SampleItem item)
        {
            Items.Add(item);
        }

        public void Remove(SampleItem item)
        {
            Items.Remove(item);
        }

        public void Clear()
        {
            Items.Clear();
        }

        public bool Contains(SampleItem item)
        {
            return Items.Contains(item);
        }

        public void UpdateItems(List<SampleItem> items)
        {
            //TODO: Traducir error
            if (items == null)
                throw new DomainException("Cannot update Items with a null List of Items");

            Items = Items ?? new List<SampleItem>();
            Items.RecreateList<int, SampleItem>(items);

            Items.UpdateList<int, SampleItem>(items);

        }

        public void Delete()
        {
            throw new NotImplementedException();
        }
    }
}
