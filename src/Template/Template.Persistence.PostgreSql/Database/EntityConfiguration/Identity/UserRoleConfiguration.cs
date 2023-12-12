using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Template.Domain.Entities.Identity;

namespace Template.Persistence.PosgreSql.Database.EntityConfiguration.Identity
{
    internal class UserRoleConfiguration : IEntityTypeConfiguration<UserRole>
    {
        public void Configure(EntityTypeBuilder<UserRole> builder)
        {
            builder.ToTable("UserRoles", Context.DbSchema.auth.ToString());
        }
    }
}
