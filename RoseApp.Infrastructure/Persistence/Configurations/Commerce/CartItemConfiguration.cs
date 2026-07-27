using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RoseApp.Domain.Entities.Commerce;

namespace RoseApp.Infrastructure.Persistence.Configurations.Commerce
{
    public class CartItemConfiguration : IEntityTypeConfiguration<CartItem>
    {
        public void Configure(EntityTypeBuilder<CartItem> builder)
        {
            builder.HasKey(ci => ci.Id);

            builder.Property(ci => ci.UnitPrice)
                .HasColumnType("decimal(18,2)")
                .HasPrecision(18, 2);

            builder.Property(ci => ci.Quantity)
                .IsRequired()
                .HasDefaultValue(1);

            builder.HasOne(ci => ci.Cart)
                .WithMany(c => c.Items)
                .HasForeignKey(ci => ci.CartId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(ci => ci.Product)
                .WithMany()
                .HasForeignKey(ci => ci.ProductId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasIndex(ci => ci.CartId);
            builder.HasIndex(ci => ci.ProductId);
        }
    }
}