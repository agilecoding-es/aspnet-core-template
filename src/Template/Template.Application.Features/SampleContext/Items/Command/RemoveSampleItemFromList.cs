using MediatR;
using Template.Application.Contracts;
using Template.Application.Features.SampleContext.Contracts;
using Template.Domain.Entities.Shared;

namespace Template.Application.Features.SampleContext.Items.Command
{
    public static class RemoveSampleItemFromList
    {
        public sealed record Command(int SampleListId, int SampleItemId) : IRequest<Result>;

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
                    var sampleList = await sampleListRepository.GetWithItemsAndUserAsync(request.SampleListId, cancellationToken);

                    var sampleItem = sampleList.Items.FirstOrDefault(x => x.Id == request.SampleItemId);
                    sampleList.Remove(sampleItem);

                    await unitOfWork.SaveChangesAsync(cancellationToken);

                    return Result.Success();
                }
                catch (Exception ex)
                {
                    return Result.Failure(ex);
                }
            }
        }
    }

}

