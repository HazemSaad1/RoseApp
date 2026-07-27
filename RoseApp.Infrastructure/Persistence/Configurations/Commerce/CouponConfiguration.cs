using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RoseApp.Domain.Entities.Commerce;

namespace RoseApp.Infrastructure.Persistence.Configurations.Commerce
{
    public class CouponConfiguration : IEntityTypeConfiguration<Coupon>
    {
        public void Configure(EntityTypeBuilder<Coupon> builder)
        {
            builder.HasKey(c => c.Id);

            builder.Property(c => c.Code)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(c => c.DiscountType)
                .HasConversion<string>()
                .HasMaxLength(20);

            builder.Property(c => c.DiscountValue)
                .HasColumnType("decimal(18,2)")
                .HasPrecision(18, 2);

            builder.Property(c => c.MinOrderAmount)
                .HasColumnType("decimal(18,2)")
                .HasPrecision(18, 2);

            builder.Property(c => c.MaxUses)
                .HasDefaultValue(null);

            builder.Property(c => c.UsedCount)
                .HasDefaultValue(0);

            builder.Property(c => c.IsActive)
                .HasDefaultValue(true);

            builder.HasIndex(c => c.Code).IsUnique();
            builder.HasIndex(c => c.IsActive);
            builder.HasIndex(c => c.ExpiresAt);
        }
    }
}