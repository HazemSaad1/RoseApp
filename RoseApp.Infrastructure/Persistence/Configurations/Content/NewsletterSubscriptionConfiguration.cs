using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RoseApp.Domain.Entities.Content;

namespace RoseApp.Infrastructure.Persistence.Configurations.Content;

public class NewsletterSubscriptionConfiguration
    : IEntityTypeConfiguration<NewsletterSubscription>
{
    public void Configure(EntityTypeBuilder<NewsletterSubscription> builder)
    {
        builder.HasKey(n => n.Id);

        builder.Property(n => n.Email)
            .IsRequired()
            .HasMaxLength(100);

        builder.HasIndex(n => n.Email).IsUnique();
    }
}