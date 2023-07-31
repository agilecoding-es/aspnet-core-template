using System.Linq.Expressions;
using Template.Application.Contracts.DTOs.Sample;
using Template.Domain.Entities.Sample;

namespace Template.Application.Contracts.Repositories.Sample
{
    public interface ISampleListRepository : IRepository<SampleList, int>
    {
        Task<SampleList> GetWithItemsAsync(int sampleListId, CancellationToken cancellationToken);
        Task<SampleList> GetWithItemsAndUserAsync(int sampleListId, CancellationToken cancellationToken);
    }
}
