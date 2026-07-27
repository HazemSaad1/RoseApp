using RoseApp.Domain.Abstractions;

namespace RoseApp.Domain.Entities.Catalog
{
    public class Occasion : BaseEntity<Guid>
    {
        public string Name { get; set; } = string.Empty;
        public string Slug { get; set; } = string.Empty;
        public string? PhotoUrl { get; set; }
        public ICollection<Product> Products { get; set; } = [];
    }
}
