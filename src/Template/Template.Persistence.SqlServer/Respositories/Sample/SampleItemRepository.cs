using Template.Application.Features.Sample.Contracts;
using Template.Domain.Entities.Sample;
using Template.Persistence.SqlServer.Database;

namespace Template.Persistence.SqlServer.Respositories.Sample
{
    public class SampleItemRepository : Repository<SampleItem, int>, ISampleItemRepository
    {
        public SampleItemRepository(Context context) : base(context)
        {
        }
    }
}
