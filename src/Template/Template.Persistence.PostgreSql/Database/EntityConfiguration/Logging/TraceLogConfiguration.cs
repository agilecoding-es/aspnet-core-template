using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Template.Common;
using Template.Domain.Entities.Logging;

namespace Template.Persistence.PosgreSql.Database.EntityConfiguration.Logging
{
    internal class TraceLogConfiguration : IEntityTypeConfiguration<TraceLog>
    {
        public void Configure(EntityTypeBuilder<TraceLog> builder)
        {
            builder.ToTable("Traces".ToLower(), DbSchema.log.ToString());

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
