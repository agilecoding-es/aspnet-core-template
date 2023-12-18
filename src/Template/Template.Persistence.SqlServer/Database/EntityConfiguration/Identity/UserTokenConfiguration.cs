using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Template.Common;
using Template.Domain.Entities.Identity;

namespace Template.Persistence.SqlServer.Database.EntityConfiguration.Identity
{
    internal class UserTokenConfiguration : IEntityTypeConfiguration<UserToken>
    {
        public void Configure(EntityTypeBuilder<UserToken> builder)
        {
            builder.ToTable("UserTokens", DbSchema.auth.ToString());
        }
    }
}
