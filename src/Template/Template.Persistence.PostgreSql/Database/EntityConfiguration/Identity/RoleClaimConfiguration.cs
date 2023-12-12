using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Template.Domain.Entities.Identity;

namespace Template.Persistence.PosgreSql.Database.EntityConfiguration.Identity
{
    internal class RoleClaimConfiguration : IEntityTypeConfiguration<RoleClaim>
    {
        public void Configure(EntityTypeBuilder<RoleClaim> builder)
        {
            builder.ToTable("RoleClaims", Context.DbSchema.auth.ToString());
        }
    }
}
