using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RoseApp.Domain.Entities.Commerce;

namespace RoseApp.Infrastructure.Persistence.Configurations.Commerce
{
    public class OrderItemConfiguration : IEntityTypeConfiguration<OrderItem>
    {
        public void Configure(EntityTypeBuilder<OrderItem> builder)
        {
            builder.HasKey(oi => oi.Id);

            builder.Property(oi => oi.UnitPrice)
                .HasColumnType("decimal(18,2)")
                .HasPrecision(18, 2);

            builder.Property(oi => oi.TotalPrice)
                .HasColumnType("decimal(18,2)")
                .HasPrecision(18, 2);

            builder.Property(oi => oi.Quantity)
                .IsRequired()
                .HasDefaultValue(1);

            builder.HasOne(oi => oi.Order)
                .WithMany(o => o.Items)
                .HasForeignKey(oi => oi.OrderId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(oi => oi.Product)
                .WithMany()
                .HasForeignKey(oi => oi.ProductId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasIndex(oi => oi.OrderId);
            builder.HasIndex(oi => oi.ProductId);
        }
    }
}