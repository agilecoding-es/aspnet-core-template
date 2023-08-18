using System.ComponentModel;
using Template.Domain.Entities.Abastractions;
using Template.Domain.Exceptions;

namespace Template.Domain.Entities.Sample
{
    [DisplayName("Item")]
    public class SampleItem : Entity<int>, ISoftDelete
    {
        public SampleItem() : base() { }

        public int ListId { get; private set; }
        public string Description { get; private set; }

        public static SampleItem Create(string description)
        {
            var sampleList = new SampleItem()
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
