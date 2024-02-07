using Template.Application.Contracts.Repositories;
using Template.Domain.Entities.Sample;

namespace Template.Application.Features.SampleContext.Contracts
{
    public interface ISampleItemRepository : IRepository<SampleItem, int>
    {
    }
}
