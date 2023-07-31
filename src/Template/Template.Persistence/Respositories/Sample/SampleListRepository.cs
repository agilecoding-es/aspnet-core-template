using System.Linq.Expressions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Template.Application.Contracts.DTOs.Sample;
using Template.Application.Contracts.Repositories.Sample;
using Template.Domain.Entities.Sample;
using Template.Persistence.Database;

namespace Template.Persistence.Respositories.Sample
{
    public class SampleListRepository : Repository<SampleList, int>, ISampleListRepository
    {
        public SampleListRepository(Context context) : base(context)
        {
        }

        public async Task<SampleList> GetWithItemsAsync(int sampleListId, CancellationToken cancellationToken)
        {
            var result = await context.SampleLists!
                         .Include(s => s.Items)
                         .FirstOrDefaultAsync(s => s.Id == sampleListId, cancellationToken);

            return result;
        }

        public async Task<SampleList> GetWithItemsAndUserAsync(int sampleListId, CancellationToken cancellationToken)
        {
            var result = await context.SampleLists!
                         .Include(s => s.Items)
                         .Include(s => s.User)
                         .FirstOrDefaultAsync(s => s.Id == sampleListId, cancellationToken);

            return result;
        }

    }
}
