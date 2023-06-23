using Template.Application.Contracts.DTOs.Sample;
using Template.Domain.Entities.Sample;

namespace Template.Application.Contracts.Repositories.Sample
{
    public interface ISampleListQueryRepository : IRepository
    {
        Task<SampleListWithItemsDto> GetByIdWithItemsAsync(int sampleListId, CancellationToken cancellationToken);

        Task<List<SampleListWithItemsCountDto>> ListWithItemsCountByUserAsync(string userId, CancellationToken cancellationToken);
    }
}
