using Template.Domain.Entities.Abastractions;

namespace Template.Domain.Entities.Sample
{
    public record SampleItemKey(Guid Value) : Key(Value);

    public class SampleItem : Entity<SampleItemKey>
    {
        private SampleItem(SampleItemKey id) : base(id) { }

        public SampleItemKey Id { get; set; }
        public SampleListKey ListId { get; set; }
        public string Description { get; set; }

        public static SampleItem Create(string description)
        {
            var sampleList = new SampleItem(new SampleItemKey(Guid.NewGuid()))
            {
                Description = description
            };

            return sampleList;
        }

    }
}
