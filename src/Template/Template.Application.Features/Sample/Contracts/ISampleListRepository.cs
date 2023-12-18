using Template.Application.Contracts.Repositories;
using Template.Domain.Entities.Sample;

namespace Template.Application.Features.Sample.Contracts
{
    public interface ISampleListRepository : IRepository<SampleList, int>
    {
        Task<SampleList> GetWithItemsAsync(int sampleListId, CancellationToken cancellationToken);
        Task<SampleList> GetWithItemsAndUserAsync(int sampleListId, CancellationToken cancellationToken);
    }
}
