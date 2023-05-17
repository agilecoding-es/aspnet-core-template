using Template.Domain.Entities.Abastractions;
using Template.Domain.Exceptions;

namespace Template.Domain.Entities.Sample
{
    public record SampleItemKey(Guid Value) : Key(Value);

    public class SampleItem : Entity<SampleItemKey>, ISoftDelete
    {
        public SampleItem() : base(default) { }

        private SampleItem(SampleItemKey id) : base(id) { }

        public SampleItemKey Id { get; private set; }
        public SampleListKey ListId { get; private set; }
        public string Description { get; private set; }

        public static SampleItem Create(string description)
        {
            var sampleList = new SampleItem(new SampleItemKey(Guid.NewGuid()))
            {
                Description = description
            };

            return sampleList;
        }

        public void Delete()
        {
            throw new NotImplementedException();
        }
    }
}
