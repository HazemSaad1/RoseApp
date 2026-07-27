using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RoseApp.Domain.Entities.Catalog;

namespace RoseApp.Infrastructure.Persistence.Configurations
{
    public class OccasionConfiguration : IEntityTypeConfiguration<Occasion>
    {
        public void Configure(EntityTypeBuilder<Occasion> builder)
        {
            builder.HasKey(o => o.Id);

            builder.Property(o => o.Name)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(o => o.Slug)
                .IsRequired()
                .HasMaxLength(100);

            builder.HasIndex(o => o.Slug).IsUnique();
        }
    }

}
