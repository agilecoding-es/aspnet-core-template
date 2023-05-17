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
    public static class UpdateSampleList
    {
        public sealed record Command(SampleListKey SampleListKey, string Name, string UserId, List<SampleItemDto> Items) : IRequest<Result>;

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
                    var alreadyExists = await sampleListRepository.AnyAsync(l => l.Name == request.Name
                                                                                      && l.Id != request.SampleListKey
                                                                                      && l.UserId == request.UserId,
                                                                                      cancellationToken);
                    if (alreadyExists)
                    {
                        return Result.Failure(new ValidationException(ValidationErrors.Sample.GetSampleListById.ListWithSameNameAlreadyExists));
                    }

                    var sampleList = await sampleListRepository.GetWithItemsAndUserAsync(s => s.Id== request.SampleListKey,cancellationToken);

                    sampleList.UpdateName(request.Name);
                    sampleList.UpdateItems(request.Items.Adapt<List<SampleItem>>());

                    await unitOfWork.SaveChangesAsync(cancellationToken);

                    return Result.Success();
                }
                catch (Exception ex)
                {
                    return Result<SampleListKey>.Failure(ex);
                }
            }
        }
    }

}

