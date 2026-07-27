namespace RoseApp.Application.DTOs.Wishlist;

public class WishlistItemDto
{
    public Guid Id { get; set; }
    public Guid ProductId { get; set; }
    public string ProductName { get; set; } = string.Empty;
    public string? ProductPhoto { get; set; }
    public decimal ProductPrice { get; set; }
    public int ProductStock { get; set; }
    public DateTime AddedAt { get; set; }
}