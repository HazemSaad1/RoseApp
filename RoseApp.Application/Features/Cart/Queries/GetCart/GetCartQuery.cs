using MediatR;
using RoseApp.Application.Common;
using RoseApp.Application.DTOs.Cart;

namespace RoseApp.Application.Features.Cart.Queries.GetCart;

public record GetCartQuery(Guid UserId)
    : IRequest<ResponseViewModel<CartDto>>;