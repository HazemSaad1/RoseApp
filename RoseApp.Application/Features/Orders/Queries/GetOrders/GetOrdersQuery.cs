using MediatR;
using RoseApp.Application.Common;
using RoseApp.Application.DTOs.Orders;

namespace RoseApp.Application.Features.Orders.Queries.GetOrders;

public record GetOrdersQuery(
    Guid UserId,
    int Page = 1,
    int Limit = 20,
    string? Search = null)
    : IRequest<ResponseViewModel<PaginationResult<OrderDto>>>;