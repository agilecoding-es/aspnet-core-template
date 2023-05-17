using System.Linq.Expressions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Template.Application.Contracts.DTOs.Sample;
using Template.Application.Contracts.Repositories.Sample;
using Template.Domain.Entities.Sample;
using Template.Persistence.Database;

namespace Template.Persistence.Respositories.Sample
{
    public class SampleListRepository : Repository<SampleList, SampleListKey>, ISampleListRepository
    {
        public SampleListRepository(Context context) : base(context)
        {
        }

        public async Task<SampleList> GetWithItemsAsync(Expression<Func<SampleList, bool>> expression, CancellationToken cancellationToken)
        {
            var result = await context.SampleLists!
                         .Include(s => s.Items)
                         .FirstOrDefaultAsync(expression, cancellationToken);

            return result;
        }

        public async Task<SampleList> GetWithItemsAndUserAsync(Expression<Func<SampleList, bool>> expression, CancellationToken cancellationToken)
        {
            var result = await context.SampleLists!
                         .Include(s => s.Items)
                         .Include(s => s.User)
                         .FirstOrDefaultAsync(expression, cancellationToken);

            return result;
        }

    }
}
