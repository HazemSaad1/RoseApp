using MediatR;
using RoseApp.Application.Common;

namespace RoseApp.Application.Features.Cart.Commands.AddToCart;

public record AddToCartCommand(
    Guid UserId,
    Guid ProductId,
    int Quantity) : IRequest<ResponseViewModel<string>>;