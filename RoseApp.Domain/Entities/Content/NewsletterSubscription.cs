using RoseApp.Domain.Abstractions;

namespace RoseApp.Domain.Entities.Content;

public class NewsletterSubscription : BaseEntity<Guid>
{
    public string Email { get; set; } = string.Empty;
    public DateTime SubscribedAt { get; set; } = DateTime.UtcNow;
}