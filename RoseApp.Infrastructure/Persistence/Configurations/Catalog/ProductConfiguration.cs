using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RoseApp.Domain.Entities.Catalog;

public class ProductConfiguration : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.HasKey(p => p.Id);

        builder.Property(p => p.Name)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(p => p.Slug)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(p => p.Description)
            .IsRequired();

        builder.Property(p => p.Price)
            .HasColumnType("decimal(18,2)");

        builder.HasIndex(p => p.Slug).IsUnique();

        builder.HasOne(p => p.Category)
            .WithMany()
            .HasForeignKey(p => p.CategoryId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(p => p.SubCategories)
            .WithMany(s => s.Products)
            .UsingEntity(j => j.ToTable("ProductSubCategories"));

        builder.HasMany(p => p.Occasions)
            .WithMany(o => o.Products)
            .UsingEntity(j => j.ToTable("ProductOccasions"));

        builder.HasQueryFilter(p => !p.IsDeleted);
    }
}