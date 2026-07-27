using RoseApp.Domain.Abstractions;

namespace RoseApp.Domain.Entities.Catalog
{
    public class Product : BaseEntity<Guid>
    {
        public string Name { get; set; } = string.Empty;
        public string Slug { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public int Stock { get; set; }
        public string? PhotoUrl { get; set; }
        public Guid CategoryId { get; set; }
        public Category Category { get; set; } = null!;
        public ICollection<SubCategory> SubCategories { get; set; } = [];
        public ICollection<Occasion> Occasions { get; set; } = [];
        public bool IsDeleted { get; set; }
        public DateTime? DeletedAt { get; set; }
    }
}
