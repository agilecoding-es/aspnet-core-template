using System.Linq.Expressions;
using Template.Application.Contracts.DTOs.Sample;
using Template.Domain.Entities.Sample;

namespace Template.Application.Contracts.Repositories.Sample
{
    public interface ISampleListRepository : IRepository<SampleList, SampleListKey>
    {
        Task<SampleList> GetWithItemsAsync(SampleListKey id, CancellationToken cancellationToken);
        Task<SampleList> GetWithItemsAndUserAsync(SampleListKey id, CancellationToken cancellationToken);
    }
}
