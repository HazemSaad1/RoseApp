using MediatR;
using Microsoft.EntityFrameworkCore;
using RoseApp.Application.Common;
using RoseApp.Application.Contracts;
using RoseApp.Application.Exceptions;
using RoseApp.Domain.Entities.Catalog;
using RoseApp.Domain.Entities.Commerce;

namespace RoseApp.Application.Features.Cart.Commands.UpdateCartItem;

public class UpdateCartItemCommandHandler
    : IRequestHandler<UpdateCartItemCommand, ResponseViewModel<string>>
{
    private readonly IDbContext _context;

    public UpdateCartItemCommandHandler(IDbContext context)
    {
        _context = context;
    }

    public async Task<ResponseViewModel<string>> Handle(
        UpdateCartItemCommand request,
        CancellationToken cancellationToken)
    {
        var cartItem = await _context.Set<CartItem>()
            .FirstOrDefaultAsync(ci => ci.Id == request.CartItemId, cancellationToken);

        if (cartItem is null)
            throw new NotFoundException(nameof(CartItem), request.CartItemId);

        var cart = await _context.Set<RoseApp.Domain.Entities.Commerce.Cart>()
            .FirstOrDefaultAsync(c => c.Id == cartItem.CartId
                && c.UserId == request.UserId, cancellationToken);

        if (cart is null)
            return ResponseViewModel<string>.Fail("Unauthorized", 403);

        var product = await _context.Set<Product>()
            .FirstOrDefaultAsync(p => p.Id == cartItem.ProductId, cancellationToken);

        if (product is null)
            throw new NotFoundException(nameof(Product), cartItem.ProductId);

        if (product.Stock < request.Quantity)
            return ResponseViewModel<string>.Fail(
                $"Only {product.Stock} items available in stock", 400);

        cartItem.Quantity = request.Quantity;
        _context.Entry(cartItem).State = EntityState.Modified;
        await _context.SaveChangesAsync(cancellationToken);

        return ResponseViewModel<string>.Success("Cart item updated successfully");
    }
}   