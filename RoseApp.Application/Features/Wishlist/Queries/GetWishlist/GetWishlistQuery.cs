using MediatR;
using RoseApp.Application.Common;
using RoseApp.Application.DTOs.Wishlist;

namespace RoseApp.Application.Features.Wishlist.Queries.GetWishlist;

public record GetWishlistQuery(Guid UserId)
    : IRequest<ResponseViewModel<WishlistDto>>;