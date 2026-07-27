using MediatR;
using Microsoft.AspNetCore.Mvc;
using RoseApp.API.Authorization;
using RoseApp.Application.Common;
using RoseApp.Application.Features.Subscriptions.Commands.DeleteSubscription;
using RoseApp.Application.Features.Subscriptions.Commands.Subscribe;
using RoseApp.Application.Features.Subscriptions.Commands.Unsubscribe;
using RoseApp.Application.Features.Subscriptions.Queries.GetSubscriptions;

namespace RoseApp.API.Controllers;

[ApiController]
[Route("api/subscriptions")]
public class SubscriptionsController : ControllerBase
{
    private readonly IMediator _mediator;

    public SubscriptionsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// List newsletter subscriptions (Admin only)
    /// </summary>
    [HttpGet]
    [HasPermission(Permissions.Subscriptions.View)]
    public async Task<IActionResult> GetAll(
        [FromQuery] int page = 1,
        [FromQuery] int limit = 20)
    {
        var result = await _mediator.Send(
            new GetSubscriptionsQuery(page, limit));

        return StatusCode(result.Code, result);
    }

    /// <summary>
    /// Subscribe an email to the newsletter (public)
    /// </summary>
    [HttpPost]
    public async Task<IActionResult> Subscribe(
        [FromBody] SubscribeCommand command)
    {
        var result = await _mediator.Send(command);

        return StatusCode(result.Code, result);
    }

    /// <summary>
    /// Unsubscribe by email (public)
    /// </summary>
    [HttpDelete]
    public async Task<IActionResult> Unsubscribe(
        [FromBody] UnsubscribeCommand command)
    {
        var result = await _mediator.Send(command);

        return StatusCode(result.Code, result);
    }

    /// <summary>
    /// Delete a newsletter subscription by ID (Admin only)
    /// </summary>
    [HttpDelete("{id:guid}")]
    [HasPermission(Permissions.Subscriptions.Delete)]
    public async Task<IActionResult> Delete(Guid id)
    {
        var result = await _mediator.Send(
            new DeleteSubscriptionCommand(id));

        return StatusCode(result.Code, result);
    }
}