using Mapster;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Template.Application.Contracts;
using Template.Application.Contracts.DTOs.Sample;
using Template.Application.Contracts.Repositories.Sample;
using Template.Application.Errors;
using Template.Domain.Entities.Sample;
using Template.Domain.Entities.Shared;

namespace Template.Application.Sample.Commands
{
    public static class AddSampleItemsToList
    {
        public sealed record Command(int SampleListId, List<SampleItemDto> Items) : IRequest<Result>;

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


                    var newItems = new List<SampleItem>();
                    foreach (var item in request.Items)
                    {
                        var newItem = SampleItem.Create(item.Description);
                        newItems.Add(newItem);
                    }
                    sampleList.Items.AddRange(newItems);

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
