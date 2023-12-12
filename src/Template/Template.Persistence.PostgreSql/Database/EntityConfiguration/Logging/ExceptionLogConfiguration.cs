using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Template.Domain.Entities.Sample;
using Template.Domain.Entities.Logging;

namespace Template.Persistence.PosgreSql.Database.EntityConfiguration.Logging
{
    internal class ExceptionLogConfiguration : IEntityTypeConfiguration<ExceptionLog>
    {
        public void Configure(EntityTypeBuilder<ExceptionLog> builder)
        {
            builder.ToTable("Exceptions", Context.DbSchema.log.ToString());

            builder.HasKey(b => b.Id);

            builder.Property(m => m.LevelError).IsRequired().HasMaxLength(20);
            builder.Property(m => m.SiteName).IsRequired().HasMaxLength(100);
            builder.Property(m => m.ProcessName).IsRequired().HasMaxLength(100);
            builder.Property(m => m.Message).IsRequired().HasMaxLength(500);
            builder.Property(m => m.InnerException).IsRequired().HasMaxLength(4000);
            builder.Property(m => m.StackTrace).IsRequired().HasMaxLength(4000);
            builder.Property(m => m.UserAgent).IsRequired().HasMaxLength(1000);
            builder.Property(m => m.Headers).IsRequired().HasMaxLength(4000);
            builder.Property(m => m.Properties).IsRequired().HasMaxLength(1000);
            builder.Property(m => m.ClassMethod).IsRequired().HasMaxLength(250);
            builder.Property(m => m.MachineName).IsRequired().HasMaxLength(30);
            builder.Property(m => m.MachineIP).IsRequired().HasMaxLength(30);
            builder.Property(m => m.Logger).IsRequired().HasMaxLength(250);
            builder.Property(m => m.IdUser).IsRequired().HasMaxLength(100);
            builder.Property(m => m.RemoteAddress).IsRequired().HasMaxLength(100);
            builder.Property(m => m.FullReferer).IsRequired().HasMaxLength(255);
            builder.Property(m => m.FullUrl).IsRequired().HasMaxLength(255);
        }
    }
}
