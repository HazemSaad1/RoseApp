using MediatR;
using Microsoft.EntityFrameworkCore;
using RoseApp.Application.Common;
using RoseApp.Application.Contracts;
using RoseApp.Domain.Entities.Commerce;

namespace RoseApp.Application.Features.Cart.Commands.ClearCart;

public class ClearCartCommandHandler
    : IRequestHandler<ClearCartCommand, ResponseViewModel<string>>
{
    private readonly IDbContext _context;

    public ClearCartCommandHandler(IDbContext context)
    {
        _context = context;
    }

    public async Task<ResponseViewModel<string>> Handle(
        ClearCartCommand request,
        CancellationToken cancellationToken)
    {
        var cart = await _context.Set<RoseApp.Domain.Entities.Commerce.Cart>()
            .FirstOrDefaultAsync(c => c.UserId == request.UserId, cancellationToken);

        if (cart is null)
            return ResponseViewModel<string>.Success("Cart is already empty");

        var cartItems = await _context.Set<CartItem>()
            .Where(ci => ci.CartId == cart.Id)
            .ToListAsync(cancellationToken);

        if (cartItems.Any())
        {
            _context.Set<CartItem>().RemoveRange(cartItems);
            await _context.SaveChangesAsync(cancellationToken);
        }

        return ResponseViewModel<string>.Success("Cart cleared successfully");
    }
}