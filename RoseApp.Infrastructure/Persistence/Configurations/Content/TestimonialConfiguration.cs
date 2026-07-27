using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RoseApp.Domain.Entities.Content;

namespace RoseApp.Infrastructure.Persistence.Configurations.Content
{
    public class TestimonialConfiguration : IEntityTypeConfiguration<Testimonial>
    {
        public void Configure(EntityTypeBuilder<Testimonial> builder)
        {
            builder.HasKey(t => t.Id);

            builder.Property(t => t.AuthorName)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(t => t.Content)
                .IsRequired()
                .HasMaxLength(1000);
        }
    }
}
