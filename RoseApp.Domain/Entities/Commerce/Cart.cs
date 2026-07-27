using RoseApp.Domain.Abstractions;
using RoseApp.Domain.Entities.Identity;

namespace RoseApp.Domain.Entities.Commerce
{
    public class Cart : BaseEntity<Guid>
    {
        public Guid UserId { get; set; }
        public User User { get; set; } = null!;
        public ICollection<CartItem> Items { get; set; } = [];
    }
}
