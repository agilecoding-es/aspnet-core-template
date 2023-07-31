using Template.Domain.Entities.Sample;
using MediatR;
using Template.Domain.Entities.Identity;
using Template.Application.Exceptions;
using Template.Domain.Entities.Shared;
using Template.Application.Errors;
using Template.Application.Contracts.Repositories.Sample;
using Template.Application.Contracts;
using Template.Application.Contracts.DTOs.Sample;
using Mapster;

namespace Template.Application.Sample.Commands
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
                    var sampleList = await sampleListRepository.GetWithItemsAsync(request.SampleListId, cancellationToken);

                    var sampleItem = sampleList.Items.FirstOrDefault(x => x.Id == request.SampleItemId);
                    sampleList.Items.Remove(sampleItem);

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

