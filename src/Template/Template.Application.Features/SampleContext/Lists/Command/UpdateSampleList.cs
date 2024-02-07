using MediatR;
using Microsoft.AspNetCore.Mvc.Localization;
using Template.Application.Contracts;
using Template.Application.Errors;
using Template.Application.Exceptions;
using Template.Application.Features.SampleContext.Contracts;
using Template.Domain.Entities.Shared;

namespace Template.Application.Features.SampleContext.Lists.Command
{
    public static class UpdateSampleList
    {
        public sealed record Command(int SampleListId, string Name, string UserId) : IRequest<Result>;

        public class Handler : IRequestHandler<Command, Result>
        {
            private readonly ISampleListRepository sampleListRepository;
            private readonly IUnitOfWork unitOfWork;
            private readonly IHtmlLocalizer localizer;

            public Handler(ISampleListRepository sampleListRepository, IUnitOfWork unitOfWork, IHtmlLocalizer localizer)
            {
                this.sampleListRepository = sampleListRepository;
                this.unitOfWork = unitOfWork;
                this.localizer = localizer;
            }

            public async Task<Result> Handle(Command request, CancellationToken cancellationToken)
            {
                try
                {
                    var alreadyExists = await sampleListRepository.AnyAsync(l => l.Name == request.Name
                                                                                      && l.Id != request.SampleListId
                                                                                      && l.UserId == request.UserId,
                                                                                      cancellationToken);
                    if (alreadyExists)
                    {
                        return Result.Failure(new ValidationException(localizer.GetString(ValidationErrors.Sample.GetSampleListById.ListWithSameNameAlreadyExists)));
                    }

                    var sampleList = await sampleListRepository.GetWithItemsAndUserAsync(request.SampleListId, cancellationToken);

                    sampleList.UpdateName(request.Name);

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

