using MediatR;
using Microsoft.EntityFrameworkCore;
using RoseApp.Application.Common;
using RoseApp.Application.Contracts;
using RoseApp.Application.Exceptions;
using RoseApp.Domain.Entities.UserDomain;

namespace RoseApp.Application.Features.Wishlist.Commands.RemoveFromWishlist;

public class RemoveFromWishlistCommandHandler
    : IRequestHandler<RemoveFromWishlistCommand, ResponseViewModel<string>>
{
    private readonly IDbContext _context;

    public RemoveFromWishlistCommandHandler(IDbContext context)
    {
        _context = context;
    }

    public async Task<ResponseViewModel<string>> Handle(
        RemoveFromWishlistCommand request,
        CancellationToken cancellationToken)
    {
        var wishlistItem = await _context.Set<WishlistItem>()
            .FirstOrDefaultAsync(wi => wi.Id == request.WishlistItemId,
                cancellationToken);

        if (wishlistItem is null)
            throw new NotFoundException(nameof(WishlistItem), request.WishlistItemId);

        var wishlist = await _context.Set<Domain.Entities.UserDomain.Wishlist>()
            .FirstOrDefaultAsync(w => w.Id == wishlistItem.WishlistId
                && w.UserId == request.UserId, cancellationToken);

        if (wishlist is null)
            return ResponseViewModel<string>.Fail("Unauthorized", 403);

        _context.Set<WishlistItem>().Remove(wishlistItem);
        await _context.SaveChangesAsync(cancellationToken);

        return ResponseViewModel<string>.Success(
            "Product removed from wishlist successfully");
    }
}