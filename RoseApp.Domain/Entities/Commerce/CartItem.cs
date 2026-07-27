using RoseApp.Domain.Abstractions;
using RoseApp.Domain.Entities.Catalog;

namespace RoseApp.Domain.Entities.Commerce
{
    public class CartItem : BaseEntity<Guid>
    {
        public Guid CartId { get; set; }
        public Cart Cart { get; set; } = null!;
        public Guid ProductId { get; set; }
        public Product Product { get; set; } = null!;
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
    }
}
