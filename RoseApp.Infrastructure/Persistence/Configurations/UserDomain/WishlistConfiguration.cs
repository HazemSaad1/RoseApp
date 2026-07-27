using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RoseApp.Domain.Entities.UserDomain;

namespace RoseApp.Infrastructure.Persistence.Configurations.UserDomain
{
    public class WishlistConfiguration : IEntityTypeConfiguration<Wishlist>
    {
        public void Configure(EntityTypeBuilder<Wishlist> builder)
        {
            builder.HasKey(w => w.Id);

            builder.HasOne(w => w.User)
                .WithMany()
                .HasForeignKey(w => w.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasIndex(w => w.UserId).IsUnique();
        }
    }
}
