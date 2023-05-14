using Template.Domain.Entities.Sample;
using MediatR;
using Template.Domain.Entities.Identity;
using Template.Application.Exceptions;
using Template.Domain.Entities.Shared;
using Template.Application.Errors;
using Template.Application.Contracts.Repositories.Sample;

namespace Template.Application.Sample.Commands
{
    public static class CreateSampleList
    {
        public sealed record Command(User User, string Name) : IRequest<Result>;

        public class Handler : IRequestHandler<Command, Result>
        {
            private readonly ISampleListRepository sampleListRepository;

            public Handler(ISampleListRepository sampleListRepository)
            {
                this.sampleListRepository = sampleListRepository;
            }

            public async Task<Result> Handle(Command request, CancellationToken cancellationToken)
            {
                try
                {
                    var alreadyExists = await sampleListRepository.AnyAsync(l => l.Name == request.Name && l.User == request.User, cancellationToken);
                    if (alreadyExists)
                    {
                        return Result.Failure(new ValidationException(ValidationErrors.Sample.GetSampleListById.ListWithSameNameAlreadyExists));
                    }

                    var newSampleList = SampleList.Create(request.User, request.Name);
                    await sampleListRepository.AddAsync(newSampleList, cancellationToken);

                    return Result.Success();
                }
                catch (Exception ex)
                {
                    return Result.Failure(ex);
                }
            }
        }

        //public sealed record Response(ResponseCode responseCode);
    }

}

