namespace RoseApp.Application.DTOs.Orders
{
    public class CouponInfoDto
    {
        public string Code { get; set; } = string.Empty;
        public decimal DiscountAmount { get; set; }
    }
}
