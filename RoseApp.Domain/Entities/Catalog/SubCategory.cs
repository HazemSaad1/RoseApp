using RoseApp.Domain.Abstractions;

namespace RoseApp.Domain.Entities.Catalog
{
    public class SubCategory : BaseEntity<Guid>
    {
        public string Name { get; set; } = string.Empty;
        public string Slug { get; set; } = string.Empty;
        public string? PhotoUrl { get; set; }
        public Guid CategoryId { get; set; }
        public Category Category { get; set; } = null!;
        public ICollection<Product> Products { get; set; } = [];
    }
}
