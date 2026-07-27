using RoseApp.Domain.Abstractions;
using RoseApp.Domain.Entities.Identity;

namespace RoseApp.Domain.Entities.Content
{
    public class Blog : BaseEntity<Guid>
    {
        public string Title { get; set; } = string.Empty;
        public string Slug { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public string? PhotoUrl { get; set; }
        public Guid AuthorId { get; set; }
        public User Author { get; set; } = null!;
    }
}
