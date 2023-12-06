using Template.Application.Features.Sample.Contracts.DTOs;

namespace Template.Application.Features.Sample.Contracts
{
    public interface ISampleListQueryRepository
    {
        Task<SampleListWithItemsDto> GetByIdWithItemsAsync(int sampleListId, CancellationToken cancellationToken);

        Task<List<SampleListWithItemsCountDto>> ListWithItemsCountByUserAsync(string userId, CancellationToken cancellationToken);
    }
}
