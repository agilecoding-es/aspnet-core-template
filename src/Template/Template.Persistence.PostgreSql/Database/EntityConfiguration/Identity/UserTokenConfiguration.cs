using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Template.Domain.Entities.Identity;

namespace Template.Persistence.PosgreSql.Database.EntityConfiguration.Identity
{
    internal class UserTokenConfiguration : IEntityTypeConfiguration<UserToken>
    {
        public void Configure(EntityTypeBuilder<UserToken> builder)
        {
            builder.ToTable("UserTokens", Context.DbSchema.auth.ToString());
        }
    }
}
