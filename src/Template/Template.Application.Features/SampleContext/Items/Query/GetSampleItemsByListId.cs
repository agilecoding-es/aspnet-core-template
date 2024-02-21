﻿using MediatR;
using Microsoft.AspNetCore.Mvc.Localization;
using Template.Application.Errors;
using Template.Application.Exceptions;
using Template.Application.Features.SampleContext.Contracts;
using Template.Application.Features.SampleContext.Contracts.DTOs;
using Template.Common.FunctionalExtensions;
using Template.Domain.Entities.Sample;
using Template.Domain.Entities.Shared;
using Template.Infrastructure.Caching.Redis.Abastractions;

namespace Template.Application.Features.SampleContext.Items.Query
{
    public static class GetSampleItemsByListId
    {
        public sealed record Query(int ListId, bool NoTracking = false) : IRequest<Result<List<SampleItemDto>>>, ICacheable
        {
            public string CacheKey => $"GetSampleItemsByListId[{ListId}]";
        }

        public class Handler : IRequestHandler<Query, Result<List<SampleItemDto>>>
        {
            private readonly ISampleItemQueryRepository sampleItemRepository;
            private readonly IHtmlLocalizer localizer;

            public Handler(ISampleItemQueryRepository sampleItemRepository, IHtmlLocalizer localizer)
            {
                this.sampleItemRepository = sampleItemRepository;
                this.localizer = localizer;
            }

            public async Task<Result<List<SampleItemDto>>> Handle(Query request, CancellationToken cancellationToken)
            {
                var result = await sampleItemRepository.GetItemsByListId(request.ListId, cancellationToken);

                return result == null ?
                    Result<List<SampleItemDto>>.Failure(
                        new ValidationException(localizer.GetString(ValidationErrors.Shared.EmptyList, typeof(SampleItem).GetDisplayNameOrTypeName()))
                    ) :
                    Result<List<SampleItemDto>>.Success(result.ToList());
            }
        }
    }
}
