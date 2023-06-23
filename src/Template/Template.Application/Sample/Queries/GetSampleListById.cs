using Template.Domain.Entities.Sample;
using MediatR;
using Template.Application.Abastractions;
using Template.Domain.Entities.Shared;
using Template.Application.Errors;
using Template.Application.Exceptions;
using Template.Application.Contracts.Repositories.Sample;
using Template.Application.Contracts.DTOs.Sample;
using Template.Common.Extensions;

namespace Template.Application.Sample.Queries
{
    public static class GetSampleListById
    {
        public sealed record Query(int ListId, bool NoTracking = false) : IRequest<Result<SampleListWithItemsDto>>, ICacheable
        {
            public string CacheKey => $"GetSampleListById-{ListId}";
        }

        public class Handler : IRequestHandler<Query, Result<SampleListWithItemsDto>>
        {
            private readonly ISampleListQueryRepository sampleListRepository;

            public Handler(ISampleListQueryRepository sampleListRepository)
            {
                this.sampleListRepository = sampleListRepository;
            }

            public async Task<Result<SampleListWithItemsDto>> Handle(Query request, CancellationToken cancellationToken)
            {
                var result = await sampleListRepository.GetByIdWithItemsAsync(request.ListId, cancellationToken);

                return result == null ?
                    Result<SampleListWithItemsDto>.Failure(new ValidationException(ValidationErrors.Shared.EntityNotFound(typeof(SampleList).GetDisplayNameOrTypeName()))) :
                    Result<SampleListWithItemsDto>.Success(result);
            }
        }
    }
}

