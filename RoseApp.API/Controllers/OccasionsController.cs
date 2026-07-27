using MediatR;
using Microsoft.AspNetCore.Mvc;
using RoseApp.API.Authorization;
using RoseApp.Application.Common;
using RoseApp.Application.Features.Occasions.Commands.CreateOccasion;
using RoseApp.Application.Features.Occasions.Commands.DeleteOccasion;
using RoseApp.Application.Features.Occasions.Commands.UpdateOccasion;
using RoseApp.Application.Features.Occasions.Queries.GetAllOccasions;
using RoseApp.Application.Features.Occasions.Queries.GetOccasionById;

namespace RoseApp.API.Controllers;

[ApiController]
[Route("api/occasions")]
public class OccasionsController : ControllerBase
{
    private readonly IMediator _mediator;

    public OccasionsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>Get all occasions (paginated)</summary>
    [HttpGet]
    public async Task<IActionResult> GetAll(
        [FromQuery] int page = 1,
        [FromQuery] int limit = 20,
        [FromQuery] string? search = null)
    {
        var result = await _mediator.Send(
            new GetAllOccasionsQuery(page, limit, search));

        return StatusCode(result.Code, result);
    }

    /// <summary>Get occasion by ID</summary>
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var result = await _mediator.Send(new GetOccasionByIdQuery(id));
        return StatusCode(result.Code, result);
    }

    /// <summary>Create a new occasion (Admin only)</summary>
    [HttpPost]
    [HasPermission(Permissions.Occasions.Create)]
    public async Task<IActionResult> Create([FromBody] CreateOccasionCommand command)
    {
        var result = await _mediator.Send(command);
        return StatusCode(result.Code, result);
    }

    /// <summary>Update occasion (Admin only)</summary>
    [HttpPatch("{id:guid}")]
    [HasPermission(Permissions.Occasions.Update)]
    public async Task<IActionResult> Update(
        Guid id,
        [FromBody] UpdateOccasionCommand command)
    {
        var result = await _mediator.Send(command with { Id = id });
        return StatusCode(result.Code, result);
    }

    /// <summary>Delete occasion (Admin only)</summary>
    [HttpDelete("{id:guid}")]
    [HasPermission(Permissions.Occasions.Delete)]
    public async Task<IActionResult> Delete(Guid id)
    {
        var result = await _mediator.Send(new DeleteOccasionCommand(id));
        return StatusCode(result.Code, result);
    }
}