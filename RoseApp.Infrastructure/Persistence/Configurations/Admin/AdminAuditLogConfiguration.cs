using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RoseApp.Domain.Entities.Admin;

namespace RoseApp.Infrastructure.Persistence.Configurations.Admin
{
    public class AdminAuditLogConfiguration : IEntityTypeConfiguration<AdminAuditLog>
    {
        public void Configure(EntityTypeBuilder<AdminAuditLog> builder)
        {
            builder.HasKey(a => a.Id);

            builder.Property(a => a.Action)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(a => a.EntityName)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(a => a.EntityId)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(a => a.OldValues)
                .HasColumnType("nvarchar(max)");

            builder.Property(a => a.NewValues)
                .HasColumnType("nvarchar(max)");

            builder.HasOne(a => a.Admin)
                .WithMany()
                .HasForeignKey(a => a.AdminId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasIndex(a => a.AdminId);
        }
    }
}
