using MediatR;
using Microsoft.EntityFrameworkCore;
using RoseApp.Application.Common;
using RoseApp.Application.Contracts;
using RoseApp.Domain.Entities.UserDomain;

namespace RoseApp.Application.Features.Wishlist.Commands.ClearWishlist;

public class ClearWishlistCommandHandler
    : IRequestHandler<ClearWishlistCommand, ResponseViewModel<string>>
{
    private readonly IDbContext _context;

    public ClearWishlistCommandHandler(IDbContext context)
    {
        _context = context;
    }

    public async Task<ResponseViewModel<string>> Handle(
        ClearWishlistCommand request,
        CancellationToken cancellationToken)
    {
        var wishlist = await _context.Set<Domain.Entities.UserDomain.Wishlist>()
            .FirstOrDefaultAsync(w => w.UserId == request.UserId, cancellationToken);

        if (wishlist is null)
            return ResponseViewModel<string>.Success("Wishlist is already empty");

        var items = await _context.Set<WishlistItem>()
            .Where(wi => wi.WishlistId == wishlist.Id)
            .ToListAsync(cancellationToken);

        if (!items.Any())
            return ResponseViewModel<string>.Success("Wishlist is already empty");

        _context.Set<WishlistItem>().RemoveRange(items);
        await _context.SaveChangesAsync(cancellationToken);

        return ResponseViewModel<string>.Success("Wishlist cleared successfully");
    }
}