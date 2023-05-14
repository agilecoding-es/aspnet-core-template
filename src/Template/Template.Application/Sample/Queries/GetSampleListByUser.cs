using Template.Domain.Entities.Sample;
using MediatR;
using Template.Application.Abastractions;
using Template.Domain.Entities.Shared;
using Template.Application.Errors;
using Template.Application.Exceptions;
using Template.Domain.Entities.Identity;
using Template.Application.Contracts.Repositories.Sample;

namespace Template.Application.Sample.Queries
{
    public static class GetSampleListByUser
    {
        public sealed record Query(User User) : IRequest<Result<List<(Guid Id, string Name, int ItemsCount)>>>, ICacheable
        {
            public string CacheKey => $"GetSampleListByUser-{User.Id}";
        }

        public class Handler : IRequestHandler<Query, Result<List<(Guid Id, string Name, int ItemsCount)>>>
        {
            private readonly ISampleListRepository sampleListRepository;

            public Handler(ISampleListRepository sampleListRepository)
            {
                this.sampleListRepository = sampleListRepository;
            }

            public async Task<Result<List<(Guid Id, string Name, int ItemsCount)>>> Handle(Query request, CancellationToken cancellationToken)
            {
                var result = await sampleListRepository.ListWithItemsCountAsync(l => l.UserId == request.User.Id, cancellationToken);


                return result == null ?
                    Result<List<(Guid Id, string Name, int ItemsCount)>>.Success(new List<(Guid Id, string Name, int ItemsCount)>()) :
                    Result<List<(Guid Id, string Name, int ItemsCount)>>.Success(result);
            }
        }
    }
}

