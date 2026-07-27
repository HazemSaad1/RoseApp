using MediatR;
using Microsoft.AspNetCore.Mvc;
using RoseApp.API.Authorization;
using RoseApp.Application.Common;
using RoseApp.Application.Features.Roles.Commands.AssignPermissionToRole;
using RoseApp.Application.Features.Roles.Commands.AssignRoleToUser;
using RoseApp.Application.Features.Roles.Commands.CreateRole;
using RoseApp.Application.Features.Roles.Commands.DeleteRole;
using RoseApp.Application.Features.Roles.Commands.RemovePermissionFromRole;
using RoseApp.Application.Features.Roles.Commands.RemoveRoleFromUser;
using RoseApp.Application.Features.Roles.Commands.UpdateRole;
using RoseApp.Application.Features.Roles.Queries.GetAllPermissions;
using RoseApp.Application.Features.Roles.Queries.GetAllRoles;

namespace RoseApp.API.Controllers;

/// <summary>
/// Role management endpoints
/// </summary>
[ApiController]
[Route("api/roles")]
public class RolesController : ControllerBase
{
    private readonly IMediator _mediator;

    public RolesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>Get all roles</summary>
    [HttpGet]
    [HasPermission(Permissions.Roles.View)]
    public async Task<IActionResult> GetAllRoles()
    {
        var result = await _mediator.Send(new GetAllRolesQuery());
        return StatusCode(result.Code, result);
    }

    /// <summary>Create a new role (SuperAdmin only)</summary>
    [HttpPost]
    [HasPermission(Permissions.Roles.Create)]
    public async Task<IActionResult> CreateRole(
        [FromBody] CreateRoleCommand command)
    {
        var result = await _mediator.Send(command);
        return StatusCode(result.Code, result);
    }

    /// <summary>Update role (SuperAdmin only)</summary>
    [HttpPatch("{id:guid}")]
    [HasPermission(Permissions.Roles.Update)]
    public async Task<IActionResult> UpdateRole(
        Guid id,
        [FromBody] UpdateRoleCommand command)
    {
        var result = await _mediator.Send(command with { Id = id });
        return StatusCode(result.Code, result);
    }

    /// <summary>Delete role (SuperAdmin only)</summary>
    [HttpDelete("{id:guid}")]
    [HasPermission(Permissions.Roles.Delete)]
    public async Task<IActionResult> DeleteRole(Guid id)
    {
        var result = await _mediator.Send(new DeleteRoleCommand(id));
        return StatusCode(result.Code, result);
    }

    /// <summary>Assign permission to role (SuperAdmin only)</summary>
    [HttpPost("{roleId:guid}/permissions/{permissionId:guid}")]
    [HasPermission(Permissions.Roles.AssignPermission)]
    public async Task<IActionResult> AssignPermission(
        Guid roleId,
        Guid permissionId)
    {
        var result = await _mediator.Send(
            new AssignPermissionToRoleCommand(roleId, permissionId));
        return StatusCode(result.Code, result);
    }

    /// <summary>Remove permission from role (SuperAdmin only)</summary>
    [HttpDelete("{roleId:guid}/permissions/{permissionId:guid}")]
    [HasPermission(Permissions.Roles.AssignPermission)]
    public async Task<IActionResult> RemovePermission(
        Guid roleId,
        Guid permissionId)
    {
        var result = await _mediator.Send(
            new RemovePermissionFromRoleCommand(roleId, permissionId));
        return StatusCode(result.Code, result);
    }

    /// <summary>Assign role to user (SuperAdmin only)</summary>
    [HttpPost("users/{userId:guid}/assign/{roleId:guid}")]
    [HasPermission(Permissions.Roles.AssignToUser)]
    public async Task<IActionResult> AssignRoleToUser(
        Guid userId,
        Guid roleId)
    {
        var result = await _mediator.Send(
            new AssignRoleToUserCommand(userId, roleId));
        return StatusCode(result.Code, result);
    }

    /// <summary>Remove role from user (SuperAdmin only)</summary>
    [HttpDelete("users/{userId:guid}/remove/{roleId:guid}")]
    [HasPermission(Permissions.Roles.AssignToUser)]
    public async Task<IActionResult> RemoveRoleFromUser(
        Guid userId,
        Guid roleId)
    {
        var result = await _mediator.Send(
            new RemoveRoleFromUserCommand(userId, roleId));
        return StatusCode(result.Code, result);
    }
}