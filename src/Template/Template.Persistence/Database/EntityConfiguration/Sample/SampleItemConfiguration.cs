using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Template.Domain.Entities.Sample;

namespace Template.Persistence.Database.EntityConfiguration.Sample
{
    internal class SampleItemConfiguration : IEntityTypeConfiguration<SampleItem>
    {
        public void Configure(EntityTypeBuilder<SampleItem> builder)
        {
            builder.ToTable("SampleItems", Context.DbSchema.sample.ToString());

            builder.HasKey(x => x.Id);

            builder.Property(p => p.Description)
                   .HasMaxLength(500)
                   .IsRequired();

        }
    }
}
