using Template.Application.Features.SampleContext.Contracts.DTOs;

namespace Template.Application.Features.LoggingContext.Contracts
{
    public interface IExceptionQueryRepository
    {
        Task<int> GetExceptionsCountAsync(CancellationToken cancellationToken);
    }
}
