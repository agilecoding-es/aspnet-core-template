using Template.Domain.Entities.Sample;
using MediatR;
using Template.Application.Abastractions;
using Template.Domain.Entities.Shared;
using Template.Application.Errors;
using Template.Application.Exceptions;
using Template.Application.Contracts.Repositories.Sample;
using Template.Application.Contracts.DTOs.Sample;
using Template.Common.Extensions;
using Microsoft.AspNetCore.Mvc.Localization;

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
            private readonly IHtmlLocalizer localizer;

            public Handler(ISampleListQueryRepository sampleListRepository, IHtmlLocalizer localizer)
            {
                this.sampleListRepository = sampleListRepository;
                this.localizer = localizer;
            }

            public async Task<Result<SampleListWithItemsDto>> Handle(Query request, CancellationToken cancellationToken)
            {
                var result = await sampleListRepository.GetByIdWithItemsAsync(request.ListId, cancellationToken);

                return result == null ?
                    Result<SampleListWithItemsDto>.Failure(
                        new ValidationException(localizer.GetString(ValidationErrors.Shared.EntityNotFound, typeof(SampleList).GetDisplayNameOrTypeName()))
                        ) :
                    Result<SampleListWithItemsDto>.Success(result);
            }
        }
    }
}

