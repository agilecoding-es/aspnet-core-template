using Template.Domain.Entities.Sample;
using MediatR;
using Template.Domain.Entities.Identity;
using Template.Application.Exceptions;
using Template.Domain.Entities.Shared;
using Template.Application.Errors;
using Template.Application.Contracts.Repositories.Sample;
using Template.Application.Contracts;

namespace Template.Application.Sample.Commands
{
    public static class CreateSampleList
    {
        public sealed record Command(User User, string Name) : IRequest<Result<int>>;

        public class Handler : IRequestHandler<Command, Result<int>>
        {
            private readonly ISampleListRepository sampleListRepository;
            private readonly IUnitOfWork unitOfWork;

            public Handler(ISampleListRepository sampleListRepository, IUnitOfWork unitOfWork)
            {
                this.sampleListRepository = sampleListRepository;
                this.unitOfWork = unitOfWork;
            }

            public async Task<Result<int>> Handle(Command request, CancellationToken cancellationToken)
            {
                try
                {
                    var alreadyExists = await sampleListRepository.AnyAsync(l => l.Name == request.Name 
                                                                                                     && l.UserId == request.User.Id, 
                                                                                                     cancellationToken);
                    if (alreadyExists)
                    {
                        return Result<int>.Failure(new ValidationException(ValidationErrors.Sample.GetSampleListById.ListWithSameNameAlreadyExists));
                    }

                    var newSampleList = SampleList.Create(request.User, request.Name);
                    await sampleListRepository.AddAsync(newSampleList, cancellationToken);

                    await unitOfWork.SaveChangesAsync(cancellationToken);

                    return Result<int>.Success(newSampleList.Id);
                }
                catch (Exception ex)
                {
                    return Result<int>.Failure(ex);
                }
            }
        }

        //public sealed record Response(ResponseCode responseCode);
    }

}

