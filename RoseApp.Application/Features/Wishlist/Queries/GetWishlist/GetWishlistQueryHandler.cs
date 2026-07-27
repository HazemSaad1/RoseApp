using MediatR;
using Microsoft.EntityFrameworkCore;
using RoseApp.Application.Common;
using RoseApp.Application.Contracts;
using RoseApp.Application.DTOs.Wishlist;
using RoseApp.Domain.Entities.Catalog;
using RoseApp.Domain.Entities.UserDomain;

namespace RoseApp.Application.Features.Wishlist.Queries.GetWishlist;

public class GetWishlistQueryHandler
    : IRequestHandler<GetWishlistQuery, ResponseViewModel<WishlistDto>>
{
    private readonly IDbContext _context;

    public GetWishlistQueryHandler(IDbContext context)
    {
        _context = context;
    }

    public async Task<ResponseViewModel<WishlistDto>> Handle(
        GetWishlistQuery request,
        CancellationToken cancellationToken)
    {
        var wishlist = await _context.Set<Domain.Entities.UserDomain.Wishlist>()
            .FirstOrDefaultAsync(w => w.UserId == request.UserId, cancellationToken);

        if (wishlist is null)
            return ResponseViewModel<WishlistDto>.Success(new WishlistDto
            {
                Items = []
            });

        var wishlistItems = await _context.Set<WishlistItem>()
            .Where(wi => wi.WishlistId == wishlist.Id)
            .ToListAsync(cancellationToken);

        var productIds = wishlistItems.Select(wi => wi.ProductId).ToList();

        var products = await _context.Set<Product>()
            .Where(p => productIds.Contains(p.Id))
            .ToListAsync(cancellationToken);

        var productsDict = products.ToDictionary(p => p.Id);

        var itemDtos = wishlistItems.Select(wi =>
        {
            productsDict.TryGetValue(wi.ProductId, out var product);
            return new WishlistItemDto
            {
                Id = wi.Id,
                ProductId = wi.ProductId,
                ProductName = product?.Name ?? string.Empty,
                ProductPhoto = product?.PhotoUrl,
                ProductPrice = product?.Price ?? 0,
                ProductStock = product?.Stock ?? 0,
                AddedAt = wi.AddedAt
            };
        }).ToList();

        return ResponseViewModel<WishlistDto>.Success(new WishlistDto
        {
            Id = wishlist.Id,
            Items = itemDtos
        });
    }
}