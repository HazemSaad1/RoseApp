using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RoseApp.Domain.Entities.Content;

namespace RoseApp.Infrastructure.Persistence.Configurations.Content
{
    public class BlogConfiguration : IEntityTypeConfiguration<Blog>
    {
        public void Configure(EntityTypeBuilder<Blog> builder)
        {
            builder.HasKey(b => b.Id);

            builder.Property(b => b.Title)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(b => b.Slug)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(b => b.Content)
                .IsRequired();

            builder.HasIndex(b => b.Slug).IsUnique();

            builder.HasOne(b => b.Author)
                .WithMany()
                .HasForeignKey(b => b.AuthorId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
