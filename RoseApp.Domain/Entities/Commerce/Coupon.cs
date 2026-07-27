using RoseApp.Domain.Abstractions;
using RoseApp.Domain.Enums;

namespace RoseApp.Domain.Entities.Commerce
{
    public class Coupon : BaseEntity<Guid>
    {
        public string Code { get; set; } = string.Empty;
        public DiscountType DiscountType { get; set; }
        public decimal DiscountValue { get; set; }
        public decimal? MinOrderAmount { get; set; }
        public int? MaxUses { get; set; }
        public int UsedCount { get; set; }
        public DateTime? ExpiresAt { get; set; }
        public bool IsActive { get; set; } = true;
    }
}
