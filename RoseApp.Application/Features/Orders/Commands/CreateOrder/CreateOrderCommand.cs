using MediatR;
using RoseApp.Application.Common;
using RoseApp.Application.DTOs.Orders;

namespace RoseApp.Application.Features.Orders.Commands.CreateOrder;

public record CreateOrderCommand(
    Guid UserId,
    Guid AddressId,
    string? CouponCode,
    string? Notes) : IRequest<ResponseViewModel<OrderDto>>;