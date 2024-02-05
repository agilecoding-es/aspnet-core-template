using Template.Application.Features.SampleContext.Contracts.DTOs;

namespace Template.Application.Features.SampleContext.Contracts
{
    public interface ISampleItemQueryRepository
    {
        Task<IEnumerable<SampleItemDto>> GetItemsByListId(int listId, CancellationToken cancellationToken);
    }
}
