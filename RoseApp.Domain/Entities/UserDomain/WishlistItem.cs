using RoseApp.Domain.Abstractions;
using RoseApp.Domain.Entities.Catalog;

namespace RoseApp.Domain.Entities.UserDomain
{
    public class WishlistItem : BaseEntity<Guid>
    {
        public Guid WishlistId { get; set; }
        public Wishlist Wishlist { get; set; } = null!;
        public Guid ProductId { get; set; }
        public Product Product { get; set; } = null!;
        public DateTime AddedAt { get; set; }
    }
}
