using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RoseApp.Domain.Entities.Identity;

namespace RoseApp.Infrastructure.Persistence.Configurations.Identity
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.HasKey(u => u.Id);

            builder.Property(u => u.FirstName)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(u => u.LastName)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(u => u.UserName)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(u => u.Email)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(u => u.PasswordHash)
                .IsRequired()
                .HasMaxLength(255);

            builder.Property(u => u.PhoneNumber)
                .HasMaxLength(20);

            builder.Property(u => u.PhotoUrl)
                .HasMaxLength(500);

            builder.Property(u => u.IsEmailVerified)
                .HasDefaultValue(false);

            builder.HasIndex(u => u.Email).IsUnique();
            builder.HasIndex(u => u.UserName).IsUnique();
            builder.HasIndex(u => new { u.Email, u.IsEmailVerified });
        }
    }
}