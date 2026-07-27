using MediatR;
using RoseApp.Application.Common;

namespace RoseApp.Application.Features.Wishlist.Commands.AddToWishlist;

public record AddToWishlistCommand(
    Guid UserId,
    Guid ProductId) : IRequest<ResponseViewModel<string>>;