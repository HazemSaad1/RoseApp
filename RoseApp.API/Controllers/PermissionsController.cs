using MediatR;
using Microsoft.AspNetCore.Mvc;
using RoseApp.API.Authorization;
using RoseApp.Application.Common;
using RoseApp.Application.Features.Roles.Queries.GetAllPermissions;

namespace RoseApp.API.Controllers;

/// <summary>
/// Permission management endpoints
/// </summary>
[ApiController]
[Route("api/permissions")]
public class PermissionsController : ControllerBase
{
    private readonly IMediator _mediator;

    public PermissionsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>Get all available permissions (SuperAdmin only)</summary>
    [HttpGet]
    [HasPermission(Permissions.Roles.View)]
    public async Task<IActionResult> GetAllPermissions()
    {
        var result = await _mediator.Send(new GetAllPermissionsQuery());
        return StatusCode(result.Code, result);
    }
}