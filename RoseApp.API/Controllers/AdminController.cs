using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RoseApp.API.Authorization;
using RoseApp.Application.Common;
using RoseApp.Application.Features.Admin.Commands.ClearAuditLogs;
using RoseApp.Application.Features.Admin.Commands.DeleteAuditLog;
using RoseApp.Application.Features.Admin.Queries.GetAuditLogs;
using RoseApp.Application.Features.Admin.Queries.GetStatistics;

namespace RoseApp.API.Controllers;

[ApiController]
[Route("api/admin")]
[Authorize]
public class AdminController : ControllerBase
{
    private readonly IMediator _mediator;

    public AdminController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>Aggregated metrics for the admin statistics</summary>
    [HttpGet("statistics")]
    [HasPermission(Permissions.Admin.ViewStats)]
    public async Task<IActionResult> GetStatistics()
    {
        var result = await _mediator.Send(new GetStatisticsQuery());
        return StatusCode(result.Code, result);
    }

    /// <summary>List admin audit log entries</summary>
    [HttpGet("audit-logs")]
    [HasPermission(Permissions.Admin.ViewAuditLog)]
    public async Task<IActionResult> GetAuditLogs(
        [FromQuery] int page = 1,
        [FromQuery] int limit = 20)
    {
        var result = await _mediator.Send(new GetAuditLogsQuery(page, limit));
        return StatusCode(result.Code, result);
    }

    /// <summary>Clear all admin audit log entries</summary>
    [HttpDelete("audit-logs")]
    [HasPermission(Permissions.Admin.DeleteAuditLog)]
    public async Task<IActionResult> ClearAuditLogs()
    {
        var result = await _mediator.Send(new ClearAuditLogsCommand());
        return StatusCode(result.Code, result);
    }

    /// <summary>Delete one admin audit log entry</summary>
    [HttpDelete("audit-logs/{id:guid}")]
    [HasPermission(Permissions.Admin.DeleteAuditLog)]
    public async Task<IActionResult> DeleteAuditLog(Guid id)
    {
        var result = await _mediator.Send(new DeleteAuditLogCommand(id));
        return StatusCode(result.Code, result);
    }
}