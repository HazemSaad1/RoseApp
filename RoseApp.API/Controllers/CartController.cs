using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RoseApp.Application.Common;
using RoseApp.Application.Features.Cart.Commands.AddToCart;
using RoseApp.Application.Features.Cart.Commands.ClearCart;
using RoseApp.Application.Features.Cart.Commands.RemoveCartItem;
using RoseApp.Application.Features.Cart.Commands.UpdateCartItem;
using RoseApp.Application.Features.Cart.Queries.GetCart;

namespace RoseApp.API.Controllers;

[ApiController]
[Route("api/cart")]
[Authorize]
public class CartController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly ICurrentUserService _currentUserService;

    public CartController(
        IMediator mediator,
        ICurrentUserService currentUserService)
    {
        _mediator = mediator;
        _currentUserService = currentUserService;
    }

    /// <summary>Get current user's cart</summary>
    [HttpGet]
    public async Task<IActionResult> GetCart()
    {
        var result = await _mediator.Send(
            new GetCartQuery(_currentUserService.UserId));
        return StatusCode(result.Code, result);
    }

    /// <summary>Add item to cart</summary>
    [HttpPost]
    public async Task<IActionResult> AddToCart([FromBody] AddToCartCommand command)
    {
        var result = await _mediator.Send(
            command with { UserId = _currentUserService.UserId });
        return StatusCode(result.Code, result);
    }

    /// <summary>Update cart item quantity</summary>
    [HttpPatch("{id:guid}")]
    public async Task<IActionResult> UpdateCartItem(
        Guid id, [FromBody] UpdateCartItemCommand command)
    {
        var result = await _mediator.Send(
            command with { UserId = _currentUserService.UserId, CartItemId = id });
        return StatusCode(result.Code, result);
    }

    /// <summary>Remove item from cart</summary>
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> RemoveCartItem(Guid id)
    {
        var result = await _mediator.Send(
            new RemoveCartItemCommand(_currentUserService.UserId, id));
        return StatusCode(result.Code, result);
    }

    /// <summary>Clear entire cart</summary>
    [HttpDelete]
    public async Task<IActionResult> ClearCart()
    {
        var result = await _mediator.Send(
            new ClearCartCommand(_currentUserService.UserId));
        return StatusCode(result.Code, result);
    }
}