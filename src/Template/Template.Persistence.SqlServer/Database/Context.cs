using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;
using Template.Domain.Entities.Identity;
using Template.Domain.Entities.Sample;

namespace Template.Persistence.SqlServer.Database
{
    public class Context : IdentityDbContext<User, Role, string, UserClaim, UserRole, UserLogin, RoleClaim, UserToken>
    {
        public DbSet<SampleList> SampleLists { get; set; }
        public DbSet<SampleItem> SampleItems { get; set; }

        public Context() { }

        public Context(DbContextOptions<Context> options)
            : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.ApplyConfigurationsFromAssembly(PersistenceSqlServerAssembly.Assembly);
        }
    }
}