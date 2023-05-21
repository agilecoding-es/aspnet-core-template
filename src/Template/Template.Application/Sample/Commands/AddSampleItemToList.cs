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
    public static class AddSampleItemToList
    {
        public sealed record Command(SampleListKey SampleListKey, SampleItemDto Item) : IRequest<Result>;

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
                    var sampleList = await sampleListRepository.GetWithItemsAsync(request.SampleListKey, cancellationToken);

                    sampleList.Items.Add(request.Item.Adapt<SampleItem>());

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

