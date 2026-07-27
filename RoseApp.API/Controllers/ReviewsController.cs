using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RoseApp.Application.Common;
using RoseApp.Application.Features.Reviews.Commands.CreateReview;
using RoseApp.Application.Features.Reviews.Commands.DeleteReview;
using RoseApp.Application.Features.Reviews.Commands.UpdateReview;
using RoseApp.Application.Features.Reviews.Queries.GetReviews;

namespace RoseApp.API.Controllers;

[ApiController]
[Route("api/reviews")]
public class ReviewsController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly ICurrentUserService _currentUserService;

    public ReviewsController(
        IMediator mediator,
        ICurrentUserService currentUserService)
    {
        _mediator = mediator;
        _currentUserService = currentUserService;
    }

    /// <summary>Get reviews (paginated, optionally filter by product or user)</summary>
    [HttpGet]
    public async Task<IActionResult> GetReviews(
        [FromQuery] int page = 1,
        [FromQuery] int limit = 20,
        [FromQuery] Guid? productId = null,
        [FromQuery] Guid? userId = null,
        [FromQuery] string? search = null)
    {
        var result = await _mediator.Send(
            new GetReviewsQuery(page, limit, productId, userId, search));

        return StatusCode(result.Code, result);
    }

    /// <summary>Create a product review</summary>
    [HttpPost]
    [Authorize]
    public async Task<IActionResult> Create([FromBody] CreateReviewCommand command)
    {
        var result = await _mediator.Send(
            command with { UserId = _currentUserService.UserId });
        return StatusCode(result.Code, result);
    }

    /// <summary>Update review</summary>
    [HttpPatch("{id:guid}")]
    [Authorize]
    public async Task<IActionResult> Update(
        Guid id,
        [FromBody] UpdateReviewCommand command)
    {
        var result = await _mediator.Send(
            command with { ReviewId = id, UserId = _currentUserService.UserId });
        return StatusCode(result.Code, result);
    }

    /// <summary>Delete review</summary>
    [HttpDelete("{id:guid}")]
    [Authorize]
    public async Task<IActionResult> Delete(Guid id)
    {
        var result = await _mediator.Send(
            new DeleteReviewCommand(id, _currentUserService.UserId));
        return StatusCode(result.Code, result);
    }
}