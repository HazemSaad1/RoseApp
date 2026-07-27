using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RoseApp.API.Authorization;
using RoseApp.Application.Common;
using RoseApp.Application.Features.Notifications.Commands.ClearAllNotifications;
using RoseApp.Application.Features.Notifications.Commands.CreateNotification;
using RoseApp.Application.Features.Notifications.Commands.DeleteNotification;
using RoseApp.Application.Features.Notifications.Commands.MarkAllAsRead;
using RoseApp.Application.Features.Notifications.Commands.MarkAsRead;
using RoseApp.Application.Features.Notifications.Queries.GetNotifications;

namespace RoseApp.API.Controllers;

[ApiController]
[Route("api/notifications")]
[Authorize]
public class NotificationsController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly ICurrentUserService _currentUserService;

    public NotificationsController(
        IMediator mediator,
        ICurrentUserService currentUserService)
    {
        _mediator = mediator;
        _currentUserService = currentUserService;
    }

    /// <summary>Get current user's notifications (paginated)</summary>
    [HttpGet]
    public async Task<IActionResult> GetNotifications(
        [FromQuery] int page = 1,
        [FromQuery] int limit = 20,
        [FromQuery] string? search = null)
    {
        var result = await _mediator.Send(
            new GetNotificationsQuery(
                _currentUserService.UserId,
                page,
                limit,
                search));

        return StatusCode(result.Code, result);
    }

    /// <summary>Create a notification for a user (Admin only)</summary>
    [HttpPost]
    [HasPermission(Permissions.Notifications.Create)]
    public async Task<IActionResult> Create([FromBody] CreateNotificationCommand command)
    {
        var result = await _mediator.Send(command);
        return StatusCode(result.Code, result);
    }

    /// <summary>Mark all current user's notifications as read</summary>
    [HttpPatch("mark-all-read")]
    public async Task<IActionResult> MarkAllAsRead()
    {
        var result = await _mediator.Send(
            new MarkAllAsReadCommand(_currentUserService.UserId));
        return StatusCode(result.Code, result);
    }

    /// <summary>Delete all current user's notifications</summary>
    [HttpDelete("clear-all")]
    public async Task<IActionResult> ClearAll()
    {
        var result = await _mediator.Send(
            new ClearAllNotificationsCommand(_currentUserService.UserId));
        return StatusCode(result.Code, result);
    }

    /// <summary>Update notification (e.g. mark as read)</summary>
    [HttpPatch("{id:guid}")]
    public async Task<IActionResult> MarkAsRead(Guid id)
    {
        var result = await _mediator.Send(
            new MarkAsReadCommand(id, _currentUserService.UserId));
        return StatusCode(result.Code, result);
    }

    /// <summary>Delete a notification by ID</summary>
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var result = await _mediator.Send(
            new DeleteNotificationCommand(id, _currentUserService.UserId));
        return StatusCode(result.Code, result);
    }
}