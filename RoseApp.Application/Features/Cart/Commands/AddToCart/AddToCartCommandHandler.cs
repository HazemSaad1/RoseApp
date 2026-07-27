using MediatR;
using Microsoft.EntityFrameworkCore;
using RoseApp.Application.Common;
using RoseApp.Application.Contracts;
using RoseApp.Application.Exceptions;
using RoseApp.Domain.Entities.Catalog;
using RoseApp.Domain.Entities.Commerce;

namespace RoseApp.Application.Features.Cart.Commands.AddToCart;

public class AddToCartCommandHandler
    : IRequestHandler<AddToCartCommand, ResponseViewModel<string>>
{
    private readonly IDbContext _context;

    public AddToCartCommandHandler(IDbContext context)
    {
        _context = context;
    }

    public async Task<ResponseViewModel<string>> Handle(
        AddToCartCommand request,
        CancellationToken cancellationToken)
    {
        var product = await _context.Set<Product>()
            .FirstOrDefaultAsync(p => p.Id == request.ProductId, cancellationToken);

        if (product is null)
            throw new NotFoundException(nameof(Product), request.ProductId);

        if (product.Stock < request.Quantity)
            return ResponseViewModel<string>.Fail(
                $"Only {product.Stock} items available in stock", 400);

        var cart = await _context.Set<RoseApp.Domain.Entities.Commerce.Cart>()
            .FirstOrDefaultAsync(c => c.UserId == request.UserId, cancellationToken);

        if (cart is null)
        {
            cart = new RoseApp.Domain.Entities.Commerce.Cart { Id = Guid.NewGuid(), UserId = request.UserId };
            await _context.Set<RoseApp.Domain.Entities.Commerce.Cart>().AddAsync(cart, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
        }

        var existingItem = await _context.Set<CartItem>()
            .FirstOrDefaultAsync(ci =>
                ci.CartId == cart.Id &&
                ci.ProductId == request.ProductId, cancellationToken);

        if (existingItem is not null)
        {
            if (product.Stock < existingItem.Quantity + request.Quantity)
                return ResponseViewModel<string>.Fail(
                    $"Only {product.Stock} items available in stock", 400);

            existingItem.Quantity += request.Quantity;
            _context.Entry(existingItem).State = EntityState.Modified;
        }
        else
        {
            await _context.Set<CartItem>().AddAsync(new CartItem
            {
                Id = Guid.NewGuid(),
                CartId = cart.Id,
                ProductId = request.ProductId,
                Quantity = request.Quantity,
                UnitPrice = product.Price
            }, cancellationToken);
        }

        await _context.SaveChangesAsync(cancellationToken);

        return ResponseViewModel<string>.Success("Item added to cart successfully");
    }
}