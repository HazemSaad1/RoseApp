using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RoseApp.Application.Common;
using RoseApp.Application.Features.Wishlist.Commands.AddToWishlist;
using RoseApp.Application.Features.Wishlist.Commands.ClearWishlist;
using RoseApp.Application.Features.Wishlist.Commands.RemoveFromWishlist;
using RoseApp.Application.Features.Wishlist.Queries.GetWishlist;

namespace RoseApp.API.Controllers;

[ApiController]
[Route("api/wishlist")]
[Authorize]
public class WishlistController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly ICurrentUserService _currentUserService;

    public WishlistController(
        IMediator mediator,
        ICurrentUserService currentUserService)
    {
        _mediator = mediator;
        _currentUserService = currentUserService;
    }

    /// <summary>Get current user's wishlist</summary>
    [HttpGet]
    public async Task<IActionResult> GetWishlist()
    {
        var result = await _mediator.Send(
            new GetWishlistQuery(_currentUserService.UserId));
        return StatusCode(result.Code, result);
    }

    /// <summary>Add product to wishlist</summary>
    [HttpPost]
    public async Task<IActionResult> AddToWishlist(
        [FromBody] AddToWishlistCommand command)
    {
        var result = await _mediator.Send(
            command with { UserId = _currentUserService.UserId });
        return StatusCode(result.Code, result);
    }

    /// <summary>Remove item from wishlist</summary>
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> RemoveFromWishlist(Guid id)
    {
        var result = await _mediator.Send(
            new RemoveFromWishlistCommand(_currentUserService.UserId, id));
        return StatusCode(result.Code, result);
    }

    /// <summary>Clear entire wishlist</summary>
    [HttpDelete]
    public async Task<IActionResult> ClearWishlist()
    {
        var result = await _mediator.Send(
            new ClearWishlistCommand(_currentUserService.UserId));
        return StatusCode(result.Code, result);
    }
}