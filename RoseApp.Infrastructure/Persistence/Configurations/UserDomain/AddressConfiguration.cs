using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RoseApp.Domain.Entities.UserDomain;

namespace RoseApp.Infrastructure.Persistence.Configurations.UserDomain
{
    public class AddressConfiguration : IEntityTypeConfiguration<Address>
    {
        public void Configure(EntityTypeBuilder<Address> builder)
        {
            builder.HasKey(a => a.Id);

            builder.Property(a => a.Label).HasMaxLength(50);
            builder.Property(a => a.Street).IsRequired().HasMaxLength(200);
            builder.Property(a => a.City).IsRequired().HasMaxLength(100);
            builder.Property(a => a.State).IsRequired().HasMaxLength(100);
            builder.Property(a => a.Country).IsRequired().HasMaxLength(100);
            builder.Property(a => a.ZipCode).HasMaxLength(20);

            builder.HasOne(a => a.User)
                .WithMany()
                .HasForeignKey(a => a.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
