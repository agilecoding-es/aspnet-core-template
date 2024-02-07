using MediatR;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Template.Domain.Entities.Abastractions;
using Template.Domain.Entities.Identity;
using Template.Domain.Entities.Sample;

namespace Template.Persistence.PosgreSql.Database
{
    public class Context : IdentityDbContext<User, Role, string, UserClaim, UserRole, UserLogin, RoleClaim, UserToken>
    {
        private readonly IMediator mediator;

        public DbSet<SampleList> SampleLists { get; set; }
        public DbSet<SampleItem> SampleItems { get; set; }

        public Context() { }

        public Context(DbContextOptions<Context> options, IMediator mediator)
            : base(options)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.ApplyConfigurationsFromAssembly(PersistencePostgreAssembly.Assembly);
        }

        public override async Task<int> SaveChangesAsync(
        CancellationToken cancellationToken = default)
        {
            // When should you publish domain events?
            //
            // 1. BEFORE calling SaveChangesAsync
            //     - domain events are part of the same transaction
            //     - immediate consistency
            // 2. AFTER calling SaveChangesAsync
            //     - domain events are a separate transaction
            //     - eventual consistency
            //     - handlers can fail

            var result = await base.SaveChangesAsync(cancellationToken);

            await PublishDomainEventsAsync();

            return result;
        }

        private async Task PublishDomainEventsAsync()
        {
            var domainEvents = ChangeTracker
                .Entries<IEntity>()
                .Select(entry => entry.Entity)
                .SelectMany(entity =>
                {
                    var domainEvents = entity.DomainEvents.ToList();

                    entity.ClearDomainEvents();

                    return domainEvents;
                })
                .ToList();

            foreach (var domainEvent in domainEvents)
            {
                await mediator.Publish(domainEvent);
            }
        }
    }
}