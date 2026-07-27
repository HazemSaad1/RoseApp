using MediatR;
using Microsoft.EntityFrameworkCore;
using RoseApp.Application.Common;
using RoseApp.Application.Contracts;
using RoseApp.Application.Exceptions;
using RoseApp.Domain.Entities.Commerce;

namespace RoseApp.Application.Features.Cart.Commands.RemoveCartItem;

public class RemoveCartItemCommandHandler
    : IRequestHandler<RemoveCartItemCommand, ResponseViewModel<string>>
{
    private readonly IDbContext _context;

    public RemoveCartItemCommandHandler(IDbContext context)
    {
        _context = context;
    }

    public async Task<ResponseViewModel<string>> Handle(
        RemoveCartItemCommand request,
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

        _context.Set<CartItem>().Remove(cartItem);
        await _context.SaveChangesAsync(cancellationToken);

        return ResponseViewModel<string>.Success("Item removed from cart successfully");
    }
}