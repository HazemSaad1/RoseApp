using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RoseApp.API.Authorization;
using RoseApp.Application.Common;
using RoseApp.Application.Features.Coupons.Commands.CreateCoupon;
using RoseApp.Application.Features.Coupons.Commands.DeleteCoupon;
using RoseApp.Application.Features.Coupons.Commands.UpdateCoupon;
using RoseApp.Application.Features.Coupons.Queries.GetAllCoupons;
using RoseApp.Application.Features.Coupons.Queries.GetCouponById;

namespace RoseApp.API.Controllers;

[ApiController]
[Route("api/coupons")]
public class CouponsController : ControllerBase
{
    private readonly IMediator _mediator;

    public CouponsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>Get all coupons (paginated)</summary>
    [HttpGet]
    [Authorize]
    public async Task<IActionResult> GetAll(
        [FromQuery] int page = 1,
        [FromQuery] int limit = 20,
        [FromQuery] string? search = null)
    {
        var result = await _mediator.Send(
            new GetAllCouponsQuery(page, limit, search));

        return StatusCode(result.Code, result);
    }

    /// <summary>Get coupon by ID</summary>
    [HttpGet("{id:guid}")]
    [Authorize]
    public async Task<IActionResult> GetById(Guid id)
    {
        var result = await _mediator.Send(new GetCouponByIdQuery(id));
        return StatusCode(result.Code, result);
    }

    /// <summary>Create a new coupon (Admin only)</summary>
    [HttpPost]
    [HasPermission(Permissions.Coupons.Create)]
    public async Task<IActionResult> Create([FromBody] CreateCouponCommand command)
    {
        var result = await _mediator.Send(command);
        return StatusCode(result.Code, result);
    }

    /// <summary>Update coupon (Admin only)</summary>
    [HttpPatch("{id:guid}")]
    [HasPermission(Permissions.Coupons.Update)]
    public async Task<IActionResult> Update(
        Guid id,
        [FromBody] UpdateCouponCommand command)
    {
        var result = await _mediator.Send(command with { Id = id });
        return StatusCode(result.Code, result);
    }

    /// <summary>Delete coupon (Admin only)</summary>
    [HttpDelete("{id:guid}")]
    [HasPermission(Permissions.Coupons.Delete)]
    public async Task<IActionResult> Delete(Guid id)
    {
        var result = await _mediator.Send(new DeleteCouponCommand(id));
        return StatusCode(result.Code, result);
    }
}