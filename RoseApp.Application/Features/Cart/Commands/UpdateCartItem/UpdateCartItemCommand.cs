using MediatR;
using RoseApp.Application.Common;

namespace RoseApp.Application.Features.Cart.Commands.UpdateCartItem;

public record UpdateCartItemCommand(
    Guid UserId,
    Guid CartItemId,
    int Quantity) : IRequest<ResponseViewModel<string>>;