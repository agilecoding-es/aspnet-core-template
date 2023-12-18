using MediatR;
using Template.Application.Contracts;
using Template.Application.Features.Sample.Contracts;
using Template.Domain.Entities.Shared;

namespace Template.Application.Features.Sample.Lists.Command
{
    public static class DeleteSampleList
    {
        public sealed record Command(int SampleListId) : IRequest<Result>;

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

