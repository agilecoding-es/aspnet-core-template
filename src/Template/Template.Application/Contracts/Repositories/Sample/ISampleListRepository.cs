using System.Linq.Expressions;
using Template.Domain.Entities.Sample;

namespace Template.Application.Contracts.Repositories.Sample
{
    public interface ISampleListRepository : IRepository<SampleList, SampleListKey>
    {
        Task<List<(Guid Id, string Name, int ItemsCount)>> ListWithItemsCountAsync(Expression<Func<SampleList, bool>> expression, CancellationToken cancellationToken);
    }
}
