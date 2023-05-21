using Microsoft.AspNetCore.Identity;
using System.ComponentModel;
using System.Reflection.Metadata;
using System.Xml.Linq;
using Template.Domain.Entities.Abastractions;
using Template.Domain.Entities.Extensions;
using Template.Domain.Entities.Identity;
using Template.Domain.Exceptions;

namespace Template.Domain.Entities.Sample
{
    public record SampleListKey(Guid Value) : Key(Value);

    [DisplayName("Sample list")]
    public class SampleList : Entity<SampleListKey>
    {
        public SampleList() : base() {
            Id = new SampleListKey(Guid.NewGuid());
        }

        public SampleList(SampleListKey id) : base(id) { }

        public string Name { get; private set; }
        public List<SampleItem> Items { get; private set; } = new List<SampleItem>();

        public string UserId { get; private set; }
        public User User { get; private set; }


        public static SampleList Create(User user, string name)
        {
            var sampleList = new SampleList(new SampleListKey(Guid.NewGuid()))
            {
                UserId = user.Id,
                Name = name
            };

            return sampleList;
        }

        public void UpdateName(string name)
        {
            Name = name;
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
            Items.RecreateList<SampleItemKey, SampleItem>(items);

            Items.UpdateList<SampleItemKey, SampleItem>(items);

        }

        public void Delete()
        {
            throw new NotImplementedException();
        }
    }
}
