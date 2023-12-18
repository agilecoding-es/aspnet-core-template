using Template.Application.Features.Sample.Contracts.DTOs;

namespace Template.Application.Features.Sample.Contracts
{
    public interface ISampleItemQueryRepository
    {
        Task<IEnumerable<SampleItemDto>> GetItemsByListId(int listId, CancellationToken cancellationToken);
    }
}
