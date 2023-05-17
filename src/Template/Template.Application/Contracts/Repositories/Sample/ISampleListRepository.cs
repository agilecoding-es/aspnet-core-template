using System.Linq.Expressions;
using Template.Application.Contracts.DTOs.Sample;
using Template.Domain.Entities.Sample;

namespace Template.Application.Contracts.Repositories.Sample
{
    public interface ISampleListRepository : IRepository<SampleList, SampleListKey>
    {
        Task<SampleList> GetWithItemsAsync(Expression<Func<SampleList, bool>> expression, CancellationToken cancellationToken);
        Task<SampleList> GetWithItemsAndUserAsync(Expression<Func<SampleList, bool>> expression, CancellationToken cancellationToken);
    }
}
