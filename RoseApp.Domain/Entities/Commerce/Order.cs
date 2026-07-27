using RoseApp.Domain.Abstractions;
using RoseApp.Domain.Entities.Identity;
using RoseApp.Domain.Entities.UserDomain;
using RoseApp.Domain.Enums;
using System.Net;

namespace RoseApp.Domain.Entities.Commerce
{
    public class Order : BaseEntity<Guid>
    {
        public Guid UserId { get; set; }
        public User User { get; set; } = null!;
        public Guid AddressId { get; set; }
        public Address Address { get; set; } = null!;
        public OrderStatus Status { get; set; } = OrderStatus.Pending;
        public decimal TotalPrice { get; set; }
        public decimal DiscountAmount { get; set; }
        public decimal FinalPrice { get; set; }
        public Guid? CouponId { get; set; }
        public Coupon? Coupon { get; set; }
        public string? Notes { get; set; }
        public ICollection<OrderItem> Items { get; set; } = [];
        public Payment? Payment { get; set; }
    }
}
