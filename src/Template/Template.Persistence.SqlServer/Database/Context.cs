using MediatR;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;
using Template.Domain.Entities.Abastractions;
using Template.Domain.Entities.Identity;
using Template.Domain.Entities.Sample;

namespace Template.Persistence.SqlServer.Database
{
    public class Context : IdentityDbContext<User, Role, string, UserClaim, UserRole, UserLogin, RoleClaim, UserToken>
    {
        private readonly IMediator mediator;

        public Context()
        {
        }

        public Context(DbContextOptions<Context> options, IMediator mediator)
            : base(options)
        {
            this.mediator = mediator;
        }

        public DbSet<SampleList> SampleLists { get; set; }
        public DbSet<SampleItem> SampleItems { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.ApplyConfigurationsFromAssembly(PersistenceSqlServerAssembly.Assembly);
        }
        
        public override async Task<int> SaveChangesAsync(
        CancellationToken cancellationToken = default)
        {
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
                    var domainEvents = entity.DomainEvents;

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