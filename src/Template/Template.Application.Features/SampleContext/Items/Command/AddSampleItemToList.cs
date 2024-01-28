using MediatR;
using Template.Application.Contracts;
using Template.Application.Features.SampleContext.Contracts;
using Template.Application.Features.SampleContext.Contracts.DTOs;
using Template.Domain.Entities.Sample;
using Template.Domain.Entities.Shared;

namespace Template.Application.Features.SampleContext.Items.Command
{
    public static class AddSampleItemToList
    {
        public sealed record Command(int SampleListId, SampleItemDto Item) : IRequest<Result>;

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

                    var newItem = SampleItem.Create(request.Item.Description);
                    sampleList.Items.Add(newItem);

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

