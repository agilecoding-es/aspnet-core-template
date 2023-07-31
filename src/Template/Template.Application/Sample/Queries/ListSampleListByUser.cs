using Template.Domain.Entities.Sample;
using MediatR;
using Template.Application.Abastractions;
using Template.Domain.Entities.Shared;
using Template.Application.Errors;
using Template.Application.Exceptions;
using Template.Domain.Entities.Identity;
using Template.Application.Contracts.Repositories.Sample;
using Template.Application.Contracts.DTOs.Sample;
using Mapster;
using Template.Common.Extensions;

namespace Template.Application.Sample.Queries
{
    public static class ListSampleListByUser
    {
        public sealed record Query(User User) : IRequest<Result<List<SampleListWithItemsCountDto>>>, ICacheable
        {
            public string CacheKey => $"ListSampleListByUser-{User.Id}";
        }

        public class Handler : IRequestHandler<Query, Result<List<SampleListWithItemsCountDto>>>
        {
            private readonly ISampleListQueryRepository sampleListRepository;

            public Handler(ISampleListQueryRepository sampleListRepository)
            {
                this.sampleListRepository = sampleListRepository;
            }

            public async Task<Result<List<SampleListWithItemsCountDto>>> Handle(Query request, CancellationToken cancellationToken)
            {
                var result = await sampleListRepository.ListWithItemsCountByUserAsync(request.User.Id, cancellationToken);

                if (result.IsNullOrEmpty())
                    result = new List<SampleListWithItemsCountDto>();

                return Result<List<SampleListWithItemsCountDto>>.Success(result);
            }

        }
    }
}

