using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Template.Domain.Entities.Identity;

namespace Template.Persistence.PosgreSql.Database.EntityConfiguration.Identity
{
    internal class UserClaimConfiguration : IEntityTypeConfiguration<UserClaim>
    {
        public void Configure(EntityTypeBuilder<UserClaim> builder)
        {
            builder.ToTable("UserClaims", Context.DbSchema.auth.ToString());
        }
    }
}
