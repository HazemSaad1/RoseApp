using MediatR;
using Microsoft.EntityFrameworkCore;
using RoseApp.Application.Common;
using RoseApp.Application.Contracts;
using RoseApp.Application.Exceptions;
using RoseApp.Domain.Entities.Catalog;
using RoseApp.Domain.Entities.UserDomain;

namespace RoseApp.Application.Features.Wishlist.Commands.AddToWishlist;

public class AddToWishlistCommandHandler
    : IRequestHandler<AddToWishlistCommand, ResponseViewModel<string>>
{
    private readonly IDbContext _context;

    public AddToWishlistCommandHandler(IDbContext context)
    {
        _context = context;
    }

    public async Task<ResponseViewModel<string>> Handle(
        AddToWishlistCommand request,
        CancellationToken cancellationToken)
    {
        var productExists = await _context.Set<Product>()
            .AnyAsync(p => p.Id == request.ProductId, cancellationToken);

        if (!productExists)
            throw new NotFoundException(nameof(Product), request.ProductId);

        var wishlist = await _context.Set<Domain.Entities.UserDomain.Wishlist>()
            .FirstOrDefaultAsync(w => w.UserId == request.UserId, cancellationToken);

        if (wishlist is null)
        {
            wishlist = new Domain.Entities.UserDomain.Wishlist
            {
                Id = Guid.NewGuid(),
                UserId = request.UserId
            };
            await _context.Set<Domain.Entities.UserDomain.Wishlist>()
                .AddAsync(wishlist, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
        }

        var alreadyExists = await _context.Set<WishlistItem>()
            .AnyAsync(wi => wi.WishlistId == wishlist.Id
                && wi.ProductId == request.ProductId, cancellationToken);

        if (alreadyExists)
            return ResponseViewModel<string>.Fail(
                "Product already in wishlist", 409);

        var wishlistItem = new WishlistItem
        {
            Id = Guid.NewGuid(),
            WishlistId = wishlist.Id,
            ProductId = request.ProductId,
            AddedAt = DateTime.UtcNow
        };

        await _context.Set<WishlistItem>().AddAsync(wishlistItem, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        return ResponseViewModel<string>.Success(
            "Product added to wishlist successfully");
    }
}