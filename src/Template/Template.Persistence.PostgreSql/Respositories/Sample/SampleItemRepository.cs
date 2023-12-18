using Template.Application.Features.Sample.Contracts;
using Template.Domain.Entities.Sample;
using Template.Persistence.PosgreSql.Database;

namespace Template.Persistence.PosgreSql.Respositories.Sample
{
    public class SampleItemRepository : Repository<SampleItem, int>, ISampleItemRepository
    {
        public SampleItemRepository(Context context) : base(context)
        {
        }
    }
}
