using MediatR;
using Template.Application.Features.SampleContext.Contracts;
using Template.Application.Features.SampleContext.Contracts.DTOs;
using Template.Common.Extensions;
using Template.Domain.Entities.Identity;
using Template.Domain.Entities.Shared;
using Template.Infrastructure.Caching.Abastractions;

namespace Template.Application.Features.SampleContext.Lists.Query
{
    public static class ListSampleListByUser
    {
        public sealed record Query(User User) : IRequest<Result<List<SampleListWithItemsCountDto>>>, ICacheable
        {
            public string CacheKey => $"ListSampleListByUser[{User.Id}]";
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

