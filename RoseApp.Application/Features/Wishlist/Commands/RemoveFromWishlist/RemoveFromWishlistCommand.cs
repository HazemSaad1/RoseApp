using MediatR;
using RoseApp.Application.Common;

namespace RoseApp.Application.Features.Wishlist.Commands.RemoveFromWishlist;

public record RemoveFromWishlistCommand(
    Guid UserId,
    Guid WishlistItemId) : IRequest<ResponseViewModel<string>>;