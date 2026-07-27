using MediatR;
using RoseApp.Application.Common;
using RoseApp.Domain.Enums;

namespace RoseApp.Application.Features.Orders.Commands.UpdateOrder;

public record UpdateOrderCommand(
    Guid OrderId,
    OrderStatus Status) : IRequest<ResponseViewModel<string>>;