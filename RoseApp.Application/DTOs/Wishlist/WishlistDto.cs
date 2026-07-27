namespace RoseApp.Application.DTOs.Wishlist;

public class WishlistDto
{
    public Guid Id { get; set; }
    public List<WishlistItemDto> Items { get; set; } = [];
}
