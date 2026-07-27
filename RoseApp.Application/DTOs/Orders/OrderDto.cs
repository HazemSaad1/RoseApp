namespace RoseApp.Application.DTOs.Orders;

public class OrderDto
{
    public Guid Id { get; set; }
    public string Status { get; set; } = string.Empty;
    public decimal TotalPrice { get; set; }
    public decimal DiscountAmount { get; set; }
    public decimal FinalPrice { get; set; }
    public string? Notes { get; set; }
    public DateTime CreatedAt { get; set; }
    public AddressInfoDto Address { get; set; } = new();
    public CouponInfoDto? Coupon { get; set; }
    public List<OrderItemDto> Items { get; set; } = [];
}