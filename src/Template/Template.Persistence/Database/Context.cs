﻿using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Template.Domain.Entities.Identity;
using Template.Domain.Entities.Sample;

namespace Template.Persistence.Database
{
    public class Context : IdentityDbContext<User, Role, string, UserClaim, UserRole, UserLogin, RoleClaim, UserToken>
    {
        internal enum DbSchema
        {
            identity,
            sample
        }

        public DbSet<SampleList>? SampleLists { get; set; }
        public DbSet<SampleItem>? SampleItems { get; set; }

        public Context() { }

        public Context(DbContextOptions<Context> options)
            : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.ApplyConfigurationsFromAssembly(PersistenceAssembly.Assembly);
        }
    }
}