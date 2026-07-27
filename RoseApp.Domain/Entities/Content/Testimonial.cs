using RoseApp.Domain.Abstractions;

namespace RoseApp.Domain.Entities.Content
{
    public class Testimonial : BaseEntity<Guid>
    {
        public string AuthorName { get; set; } = string.Empty;
        public string? AuthorPhoto { get; set; }
        public string Content { get; set; } = string.Empty;
        public int Rating { get; set; }
    }
}
