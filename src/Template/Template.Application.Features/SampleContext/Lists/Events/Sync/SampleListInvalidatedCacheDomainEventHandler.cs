using MediatR;
using System.Collections.Generic;
using Template.Application.Features.IdentityContext.Services;
using Template.Application.Features.SampleContext.Items.Query;
using Template.Application.Features.SampleContext.Lists.Query;
using Template.Domain.Entities.Identity;
using Template.Domain.Entities.Sample.Events;
using Template.Infrastructure.Caching;

namespace Template.Application.Features.SampleContext.Lists.Events.Sync
{
    public class SampleListInvalidatedCacheDomainEventHandler :
        INotificationHandler<SampleListCreatedDomainEvent>,
        INotificationHandler<SampleListDeletedDomainEvent>,
        INotificationHandler<SampleListItemAddedDomainEvent>,
        INotificationHandler<SampleListItemsAddedDomainEvent>,
        INotificationHandler<SampleListItemRemovedDomainEvent>,
        INotificationHandler<SampleListItemsClearedDomainEvent>
    {
        private readonly ICacheService cacheService;

        public SampleListInvalidatedCacheDomainEventHandler(ICacheService cacheService)
        {
            this.cacheService = cacheService ?? throw new ArgumentNullException(nameof(cacheService));
        }

        public async Task Handle(SampleListCreatedDomainEvent notification, CancellationToken cancellationToken)
        {
            await RemoveListSampleListByUserCache(notification.User);
        }

        public async Task Handle(SampleListDeletedDomainEvent notification, CancellationToken cancellationToken)
        {
            await RemoveListSampleListByUserCache(notification.User);
        }

        public async Task Handle(SampleListItemAddedDomainEvent notification, CancellationToken cancellationToken)
        {
            await RemoveListSampleListByUserCache(notification.User);
            await RemoveGetSampleListByIdCache(notification.ListId);
            await RemoveGetSampleItemsByListIdCache(notification.ListId);
        }

        public async Task Handle(SampleListItemsAddedDomainEvent notification, CancellationToken cancellationToken)
        {
            await RemoveListSampleListByUserCache(notification.User);
            await RemoveGetSampleListByIdCache(notification.ListId);
            await RemoveGetSampleItemsByListIdCache(notification.ListId);
        }

        public async Task Handle(SampleListItemRemovedDomainEvent notification, CancellationToken cancellationToken)
        {
            await RemoveListSampleListByUserCache(notification.User);
            await RemoveGetSampleListByIdCache(notification.ListId);
            await RemoveGetSampleItemsByListIdCache(notification.ListId);
        }

        public async Task Handle(SampleListItemsClearedDomainEvent notification, CancellationToken cancellationToken)
        {
            await RemoveListSampleListByUserCache(notification.User);
            await RemoveGetSampleListByIdCache(notification.ListId);
            await RemoveGetSampleItemsByListIdCache(notification.ListId);
        }

        private async Task RemoveListSampleListByUserCache(User user)
        {
            var query = new ListSampleListByUser.Query(user);

            await cacheService.RemoveAsync(query.CacheKey);
        }

        private async Task RemoveGetSampleListByIdCache(int listId)
        {
            var query = new GetSampleListById.Query(listId);

            await cacheService.RemoveAsync(query.CacheKey);
        }

        private async Task RemoveGetSampleItemsByListIdCache(int listId)
        {
            var query = new GetSampleItemsByListId.Query(listId);

            await cacheService.RemoveAsync(query.CacheKey);
        }

    }
}
