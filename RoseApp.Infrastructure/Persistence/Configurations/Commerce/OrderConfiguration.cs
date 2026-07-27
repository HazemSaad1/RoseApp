using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RoseApp.Domain.Entities.Commerce;

namespace RoseApp.Infrastructure.Persistence.Configurations.Commerce
{
    public class OrderConfiguration : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.HasKey(o => o.Id);

            builder.Property(o => o.Status)
                .HasConversion<string>()
                .HasMaxLength(50);

            builder.Property(o => o.TotalPrice)
                .HasColumnType("decimal(18,2)")
                .HasPrecision(18, 2);

            builder.Property(o => o.DiscountAmount)
                .HasColumnType("decimal(18,2)")
                .HasPrecision(18, 2)
                .HasDefaultValue(0);

            builder.Property(o => o.FinalPrice)
                .HasColumnType("decimal(18,2)")
                .HasPrecision(18, 2);

            builder.Property(o => o.Notes)
                .HasMaxLength(1000);

            builder.HasIndex(o => o.UserId);
            builder.HasIndex(o => o.Status);
            builder.HasIndex(o => o.CreatedAt);

            builder.HasOne(o => o.User)
                .WithMany()
                .HasForeignKey(o => o.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(o => o.Address)
                .WithMany()
                .HasForeignKey(o => o.AddressId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(o => o.Coupon)
                .WithMany()
                .HasForeignKey(o => o.CouponId)
                .OnDelete(DeleteBehavior.SetNull);
        }
    }
}