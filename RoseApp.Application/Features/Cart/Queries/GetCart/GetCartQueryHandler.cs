using MediatR;
using Microsoft.EntityFrameworkCore;
using RoseApp.Application.Common;
using RoseApp.Application.Contracts;
using RoseApp.Application.DTOs.Cart;
using RoseApp.Domain.Entities.Catalog;
using RoseApp.Domain.Entities.Commerce;

namespace RoseApp.Application.Features.Cart.Queries.GetCart;

public class GetCartQueryHandler
    : IRequestHandler<GetCartQuery, ResponseViewModel<CartDto>>
{
    private readonly IDbContext _context;

    public GetCartQueryHandler(IDbContext context)
    {
        _context = context;
    }

    public async Task<ResponseViewModel<CartDto>> Handle(
        GetCartQuery request,
        CancellationToken cancellationToken)
    {
        var cart = await _context.Set<RoseApp.Domain.Entities.Commerce.Cart>()
            .FirstOrDefaultAsync(c => c.UserId == request.UserId, cancellationToken);

        if (cart is null)
            return ResponseViewModel<CartDto>.Success(new CartDto
            {
                Items = [],
                TotalPrice = 0
            });

        var cartItems = await _context.Set<CartItem>()
            .Where(ci => ci.CartId == cart.Id)
            .ToListAsync(cancellationToken);

        var productIds = cartItems.Select(ci => ci.ProductId).ToList();

        var products = await _context.Set<Product>()
            .Where(p => productIds.Contains(p.Id))
            .ToListAsync(cancellationToken);

        var productsDict = products.ToDictionary(p => p.Id);

        var itemDtos = cartItems.Select(ci =>
        {
            productsDict.TryGetValue(ci.ProductId, out var product);
            return new CartItemDto
            {
                Id = ci.Id,
                ProductId = ci.ProductId,
                ProductName = product?.Name ?? string.Empty,
                ProductPhoto = product?.PhotoUrl,
                UnitPrice = ci.UnitPrice,
                Quantity = ci.Quantity,
                TotalPrice = ci.UnitPrice * ci.Quantity
            };
        }).ToList();

        return ResponseViewModel<CartDto>.Success(new CartDto
        {
            Id = cart.Id,
            Items = itemDtos,
            TotalPrice = itemDtos.Sum(i => i.TotalPrice)
        });
    }
}