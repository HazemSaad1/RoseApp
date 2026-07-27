using MediatR;
using RoseApp.Application.Common;

namespace RoseApp.Application.Features.Cart.Commands.RemoveCartItem;

public record RemoveCartItemCommand(
    Guid UserId,
    Guid CartItemId) : IRequest<ResponseViewModel<string>>;