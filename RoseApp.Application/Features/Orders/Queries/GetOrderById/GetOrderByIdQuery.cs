using MediatR;
using RoseApp.Application.Common;
using RoseApp.Application.DTOs.Orders;

namespace RoseApp.Application.Features.Orders.Queries.GetOrderById;

public record GetOrderByIdQuery(
    Guid OrderId,
    Guid UserId) : IRequest<ResponseViewModel<OrderDto>>;