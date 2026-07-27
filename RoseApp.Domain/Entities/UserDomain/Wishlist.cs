using RoseApp.Domain.Abstractions;
using RoseApp.Domain.Entities.Identity;

namespace RoseApp.Domain.Entities.UserDomain
{
    public class Wishlist : BaseEntity<Guid>
    {
        public Guid UserId { get; set; }
        public User User { get; set; } = null!;
        public ICollection<WishlistItem> Items { get; set; } = [];
    }
}
