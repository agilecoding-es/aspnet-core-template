namespace Template.Domain.Entities.Sample
{
    public class SampleItem
    {
        private SampleItem()
        {

        }

        public SampleItemKey Id { get; set; }
        public SampleListKey ListId { get; set; }
        public string Description { get; set; }

        public static SampleItem Create(string description)
        {
            var sampleList = new SampleItem()
            {
                Id = new SampleItemKey(Guid.NewGuid()),
                Description = description
            };

            return sampleList;
        }

    }
}
