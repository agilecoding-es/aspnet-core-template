using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Template.Common;
using Template.Domain.Entities.Sample;

namespace Template.Persistence.SqlServer.Database.EntityConfiguration.Sample
{
    internal class SampleListConfiguration : IEntityTypeConfiguration<SampleList>
    {
        public void Configure(EntityTypeBuilder<SampleList> builder)
        {
            builder.ToTable("SampleLists", DbSchema.sample.ToString());

            builder.HasKey(x => x.Id);

            builder.Property(p => p.Name)
                   .HasMaxLength(100)
                   .IsRequired();

            builder.HasOne(p => p.User)
                   .WithMany()
                   .HasForeignKey(p => p.UserId)
                   .IsRequired()
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(p => p.Items)
                   .WithOne()
                   .HasForeignKey(p => p.ListId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasIndex(p => new { p.UserId, p.Name }).IsUnique();
        }
    }
}
