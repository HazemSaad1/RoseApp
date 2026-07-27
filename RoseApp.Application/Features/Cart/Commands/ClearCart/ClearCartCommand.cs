using MediatR;
using RoseApp.Application.Common;

namespace RoseApp.Application.Features.Cart.Commands.ClearCart;

public record ClearCartCommand(Guid UserId)
    : IRequest<ResponseViewModel<string>>;