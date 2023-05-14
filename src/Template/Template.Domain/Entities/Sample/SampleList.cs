using Microsoft.AspNetCore.Identity;
using Template.Domain.Entities.Abastractions;
using Template.Domain.Entities.Identity;

namespace Template.Domain.Entities.Sample
{
    public record SampleListKey(Guid Value) : Key(Value);

    public class SampleList : Entity<SampleListKey>
    {

        public SampleList(SampleListKey id) : base(id) { }

        public string Name { get; set; }
        public IList<SampleItem> Items { get; init; } = new List<SampleItem>();

        public string UserId { get; set; }
        public User User { get; set; }


        public static SampleList Create(User user, string name)
        {
            var sampleList = new SampleList(new SampleListKey(Guid.NewGuid()))
            {
                UserId = user.Id,
                Name = name
            };

            return sampleList;
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
    }
}
