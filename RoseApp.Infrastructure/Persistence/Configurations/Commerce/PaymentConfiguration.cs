using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RoseApp.Domain.Entities.Commerce;

namespace RoseApp.Infrastructure.Persistence.Configurations.Commerce
{
    public class PaymentConfiguration : IEntityTypeConfiguration<Payment>
    {
        public void Configure(EntityTypeBuilder<Payment> builder)
        {
            builder.HasKey(p => p.Id);

            builder.Property(p => p.Status)
                .HasConversion<string>()
                .HasMaxLength(50);

            builder.Property(p => p.Amount)
                .HasColumnType("decimal(18,2)")
                .HasPrecision(18, 2);

            builder.Property(p => p.StripePaymentIntentId)
                .IsRequired()
                .HasMaxLength(200);

            builder.HasOne(p => p.Order)
                .WithOne(o => o.Payment)
                .HasForeignKey<Payment>(p => p.OrderId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasIndex(p => p.OrderId).IsUnique();
            builder.HasIndex(p => p.StripePaymentIntentId);
        }
    }
}