using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RoseApp.Domain.Entities.Catalog;

namespace RoseApp.Infrastructure.Persistence.Configurations.Catalog
{
    public class CategoryConfiguration : IEntityTypeConfiguration<Category>
    {
        public void Configure(EntityTypeBuilder<Category> builder)
        {
            builder.HasKey(c => c.Id);

            builder.Property(c => c.Name)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(c => c.Slug)
                .IsRequired()
                .HasMaxLength(100);

            builder.HasIndex(c => c.Slug).IsUnique();
        }
    }
}
