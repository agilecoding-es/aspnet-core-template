using MediatR;
using Template.Application.Contracts;
using Template.Application.Contracts.Repositories.Sample;
using Template.Application.Errors;
using Template.Application.Exceptions;
using Template.Domain.Entities.Sample;
using Template.Domain.Entities.Shared;

namespace Template.Application.Sample.Commands
{
    public static class DeleteSampleList
    {
        public sealed record Command(int SampleListId, bool DeleteWithItems = false) : IRequest<Result>;

        public class Handler : IRequestHandler<Command, Result>
        {
            private readonly ISampleListRepository sampleListRepository;
            private readonly IUnitOfWork unitOfWork;

            public Handler(ISampleListRepository sampleListRepository, IUnitOfWork unitOfWork)
            {
                this.sampleListRepository = sampleListRepository;
                this.unitOfWork = unitOfWork;
            }

            public async Task<Result> Handle(Command request, CancellationToken cancellationToken)
            {
                try
                {

                    var sampleList = await sampleListRepository.GetWithItemsAsync(request.SampleListId, cancellationToken);
                    if (!request.DeleteWithItems && sampleList.Items.Any())
                    {
                        return Result.Failure(new ValidationException(ValidationErrors.Sample.DeleteSampleList.ListWithItems));
                    }

                    sampleListRepository.Delete(sampleList);
                    
                    await unitOfWork.SaveChangesAsync(cancellationToken);

                    return Result.Success();
                }
                catch (Exception ex)
                {
                    return Result<int>.Failure(ex);
                }
            }
        }
    }

}

