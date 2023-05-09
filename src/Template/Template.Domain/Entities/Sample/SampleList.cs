using Microsoft.AspNetCore.Identity;
using Template.Domain.Entities.Identity;

namespace Template.Domain.Entities.Sample
{
    public class SampleList
    {
        private readonly HashSet<SampleItem> _items = new HashSet<SampleItem>();

        private SampleList()
        {

        }

        public SampleListKey Id { get; set; }
        public string Name { get; set; }
        public IReadOnlyList<SampleItem> Items => _items.ToList();

        public string UserId { get; set; }
        public User User { get; set; }


        public static SampleList Create(User user, string name)
        {
            var sampleList = new SampleList()
            {
                Id = new SampleListKey(Guid.NewGuid()),
                UserId = user.Id,
                Name = name
            };

            return sampleList;
        }

        public void Add(SampleItem item)
        {
            _items.Add(item);
        }

        public void Remove(SampleItem item)
        {
            _items.Remove(item);
        }

        public void Clear()
        {
            _items.Clear();
        }

        public bool Contains(SampleItem item)
        {
            return _items.Contains(item);
        }
    }
}
