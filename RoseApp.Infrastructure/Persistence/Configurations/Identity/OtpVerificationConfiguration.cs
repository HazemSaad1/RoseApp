using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RoseApp.Domain.Entities.Identity;

namespace RoseApp.Infrastructure.Persistence.Configurations.Identity
{
    public class OtpVerificationConfiguration : IEntityTypeConfiguration<OtpVerification>
    {
        public void Configure(EntityTypeBuilder<OtpVerification> builder)
        {
            builder.HasKey(o => o.Id);

            builder.Property(o => o.Email)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(o => o.Code)
                .IsRequired()
                .HasMaxLength(10);

            builder.Property(o => o.Type)
                .HasConversion<string>();

            builder.HasIndex(o => o.Email);
        }
    }
}
