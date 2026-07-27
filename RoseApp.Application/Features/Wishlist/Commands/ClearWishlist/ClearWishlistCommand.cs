using MediatR;
using RoseApp.Application.Common;

namespace RoseApp.Application.Features.Wishlist.Commands.ClearWishlist;

public record ClearWishlistCommand(Guid UserId)
    : IRequest<ResponseViewModel<string>>;