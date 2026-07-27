using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RoseApp.API.Authorization;
using RoseApp.Application.Common;
using RoseApp.Application.Features.Orders.Commands.CreateOrder;
using RoseApp.Application.Features.Orders.Commands.UpdateOrder;
using RoseApp.Application.Features.Orders.Queries.GetOrderById;
using RoseApp.Application.Features.Orders.Queries.GetOrders;

namespace RoseApp.API.Controllers;

[ApiController]
[Route("api/orders")]
[Authorize]
public class OrdersController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly ICurrentUserService _currentUserService;

    public OrdersController(
        IMediator mediator,
        ICurrentUserService currentUserService)
    {
        _mediator = mediator;
        _currentUserService = currentUserService;
    }

    /// <summary>Get current user's orders (paginated)</summary>
    [HttpGet]
    public async Task<IActionResult> GetOrders(
        [FromQuery] int page = 1,
        [FromQuery] int limit = 20,
        [FromQuery] string? search = null)
    {
        var result = await _mediator.Send(
            new GetOrdersQuery(
                _currentUserService.UserId,
                page,
                limit,
                search));

        return StatusCode(result.Code, result);
    }

    /// <summary>Get order by ID</summary>
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var result = await _mediator.Send(
            new GetOrderByIdQuery(id, _currentUserService.UserId));
        return StatusCode(result.Code, result);
    }

    /// <summary>Create a new order</summary>
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateOrderCommand command)
    {
        var result = await _mediator.Send(
            command with { UserId = _currentUserService.UserId });
        return StatusCode(result.Code, result);
    }

    /// <summary>Update order (Admin only)</summary>
    [HttpPatch("{id:guid}")]
    [HasPermission(Permissions.Orders.Update)]
    public async Task<IActionResult> Update(
        Guid id,
        [FromBody] UpdateOrderCommand command)
    {
        var result = await _mediator.Send(command with { OrderId = id });
        return StatusCode(result.Code, result);
    }
}