using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Template.Application.Contracts.Repositories.Sample;
using Template.Domain.Entities.Sample;
using Template.Persistence.Database;

namespace Template.Persistence.Respositories
{
    public class SampleListRepository : Repository<SampleList, SampleListKey>, ISampleListRepository
    {
        public SampleListRepository(Context context) : base(context)
        {
        }

        public async Task<List<(Guid Id, string Name, int ItemsCount)>> ListWithItemsCountAsync(Expression<Func<SampleList, bool>> expression, CancellationToken cancellationToken)
        {
            var result = await context.SampleLists!
                         .Include(s => s.Items)
                         .Where(expression)
                         .Select(s => new { Id = s.Id.Value, Name = s.Name, ItemsCount = s.Items.Count })
                         .ToListAsync(cancellationToken);

            return result.ConvertAll(r => (r.Id, r.Name, r.ItemsCount));
        }
    }
}
