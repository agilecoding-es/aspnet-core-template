using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Template.DataAccess.Database
{
    public class Context : IdentityDbContext
    {
        internal enum DbSchema
        {
            identity
        }

        public Context(DbContextOptions<Context> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.ApplyConfigurationsFromAssembly(typeof(DataAccessAssembly).Assembly);
        }
    }
}