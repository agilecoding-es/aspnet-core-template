﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Template.Common;
using Template.Domain.Entities.Identity;

namespace Template.Persistence.PosgreSql.Database.EntityConfiguration.Identity
{
    internal class UserLoginConfiguration : IEntityTypeConfiguration<UserLogin>
    {
        public void Configure(EntityTypeBuilder<UserLogin> builder)
        {
            builder.ToTable("UserLogins".ToLower(), DbSchema.auth.ToString());
        }
    }
}
