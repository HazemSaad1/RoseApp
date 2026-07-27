using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RoseApp.Domain.Entities.Catalog;

namespace RoseApp.Infrastructure.Persistence.Configurations.Catalog
{
    public class SubCategoryConfiguration : IEntityTypeConfiguration<SubCategory>
    {
        public void Configure(EntityTypeBuilder<SubCategory> builder)
        {
            builder.HasKey(s => s.Id);

            builder.Property(s => s.Name)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(s => s.Slug)
                .IsRequired()
                .HasMaxLength(100);

            builder.HasIndex(s => s.Slug).IsUnique();

            builder.HasOne(s => s.Category)
                .WithMany(c => c.SubCategories)
                .HasForeignKey(s => s.CategoryId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
