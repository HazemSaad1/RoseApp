using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RoseApp.Domain.Entities.UserDomain;

namespace RoseApp.Infrastructure.Persistence.Configurations.UserDomain
{
    public class ReviewConfiguration : IEntityTypeConfiguration<Review>
    {
        public void Configure(EntityTypeBuilder<Review> builder)
        {
            builder.HasKey(r => r.Id);

            builder.Property(r => r.Rating)
                .IsRequired()
                .HasDefaultValue(5);

            builder.Property(r => r.Comment)
                .HasMaxLength(1000);

            builder.HasOne(r => r.User)
                .WithMany()
                .HasForeignKey(r => r.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(r => r.Product)
                .WithMany()
                .HasForeignKey(r => r.ProductId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasIndex(r => new { r.UserId, r.ProductId }).IsUnique();
            builder.HasIndex(r => r.Rating);
        }
    }
}