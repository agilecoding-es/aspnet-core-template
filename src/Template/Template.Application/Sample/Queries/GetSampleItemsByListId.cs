using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Template.Application.Abastractions;
using Template.Application.Contracts.DTOs.Sample;
using Template.Application.Contracts.Repositories.Sample;
using Template.Application.Errors;
using Template.Application.Exceptions;
using Template.Common.Extensions;
using Template.Domain.Entities.Sample;
using Template.Domain.Entities.Shared;

namespace Template.Application.Sample.Queries
{
    public static class GetSampleItemsByListId
    {
        public sealed record Query(int ListId, bool NoTracking = false) : IRequest<Result<List<SampleItemDto>>>, ICacheable
        {
            public string CacheKey => $"GetSampleItemsByListId-{ListId}";
        }

        public class Handler : IRequestHandler<Query, Result<List<SampleItemDto>>>
        {
            private readonly ISampleItemQueryRepository sampleItemRepository;

            public Handler(ISampleItemQueryRepository sampleItemRepository)
            {
                this.sampleItemRepository = sampleItemRepository;
            }

            public async Task<Result<List<SampleItemDto>>> Handle(Query request, CancellationToken cancellationToken)
            {
                var result = await sampleItemRepository.GetItemsByListId(request.ListId, cancellationToken);

                return result == null ?
                    Result<List<SampleItemDto>>.Failure(new ValidationException(ValidationErrors.Shared.EmptyList(typeof(SampleItem).GetDisplayNameOrTypeName()))) :
                    Result<List<SampleItemDto>>.Success(result.ToList());
            }
        }
    }
}
