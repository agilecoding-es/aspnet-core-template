using Template.Domain.Entities.Sample;
using MediatR;
using Template.Application.Abastractions;
using Template.Domain.Entities.Shared;
using Template.Application.Errors;
using Template.Application.Exceptions;
using Template.Application.Contracts.Repositories.Sample;

namespace Template.Application.Sample.Queries
{
    public static class GetSampleListById
    {
        public sealed record Query(SampleListKey id) : IRequest<Result<Response>>, ICacheable
        {
            public string CacheKey => $"GetSampleListById-{id.Value}";
        }

        public class Handler : IRequestHandler<Query, Result<Response>>
        {
            private readonly ISampleListRepository sampleListRepository;

            public Handler(ISampleListRepository sampleListRepository)
            {
                this.sampleListRepository = sampleListRepository;
            }

            public async Task<Result<Response>> Handle(Query request, CancellationToken cancellationToken)
            {
                var result = await sampleListRepository.GetByIdAsync(request.id, cancellationToken);

                return result == null ?
                    Result<Response>.Failure(new ValidationException(ValidationErrors.Shared.EntityNotFound(nameof(SampleList)))) :
                    Result<Response>.Success(new Response(result.Name, result.Items.Count()));
            }
        }

        public sealed record Response(string Name, int ItemsCount);
    }
}

