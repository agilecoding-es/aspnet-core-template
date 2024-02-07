using Template.Application.Features.SampleContext.Contracts.DTOs;

namespace Template.Application.Features.SampleContext.Contracts
{
    public interface ISampleListQueryRepository
    {
        Task<SampleListWithItemsDto> GetByIdWithItemsAsync(int sampleListId, CancellationToken cancellationToken);

        Task<List<SampleListWithItemsCountDto>> ListWithItemsCountByUserAsync(string userId, CancellationToken cancellationToken);
    }
}
