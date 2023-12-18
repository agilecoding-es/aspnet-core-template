using Template.Application.Features.Sample.Contracts.DTOs;

namespace Template.Application.Features.Logging.Contracts
{
    public interface IExceptionQueryRepository
    {
        Task<int> GetExceptionsCountAsync(CancellationToken cancellationToken);
    }
}
