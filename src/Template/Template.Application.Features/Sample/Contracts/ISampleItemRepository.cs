using Template.Application.Contracts.Repositories;
using Template.Domain.Entities.Sample;

namespace Template.Application.Features.Sample.Contracts
{
    public interface ISampleItemRepository : IRepository<SampleItem, int>
    {
    }
}
