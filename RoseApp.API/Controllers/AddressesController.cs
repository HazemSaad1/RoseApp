using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RoseApp.Application.Common;
using RoseApp.Application.Features.Addresses.Commands.CreateAddress;
using RoseApp.Application.Features.Addresses.Commands.DeleteAddress;
using RoseApp.Application.Features.Addresses.Commands.UpdateAddress;
using RoseApp.Application.Features.Addresses.Queries.GetAddressById;
using RoseApp.Application.Features.Addresses.Queries.GetAddresses;

namespace RoseApp.API.Controllers;

[ApiController]
[Route("api/addresses")]
[Authorize]
public class AddressesController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly ICurrentUserService _currentUserService;

    public AddressesController(
        IMediator mediator,
        ICurrentUserService currentUserService)
    {
        _mediator = mediator;
        _currentUserService = currentUserService;
    }

    /// <summary>Get current user's addresses</summary>
    [HttpGet]
    public async Task<IActionResult> GetAddresses()
    {
        var result = await _mediator.Send(
            new GetAddressesQuery(_currentUserService.UserId));
        return StatusCode(result.Code, result);
    }

    /// <summary>Get address by ID</summary>
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var result = await _mediator.Send(
            new GetAddressByIdQuery(_currentUserService.UserId, id));
        return StatusCode(result.Code, result);
    }

    /// <summary>Create a new address</summary>
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateAddressCommand command)
    {
        var result = await _mediator.Send(
            command with { UserId = _currentUserService.UserId });
        return StatusCode(result.Code, result);
    }

    /// <summary>Update address</summary>
    [HttpPatch("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateAddressCommand command)
    {
        var result = await _mediator.Send(
            command with { UserId = _currentUserService.UserId, AddressId = id });
        return StatusCode(result.Code, result);
    }

    /// <summary>Delete address</summary>
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var result = await _mediator.Send(
            new DeleteAddressCommand(_currentUserService.UserId, id));
        return StatusCode(result.Code, result);
    }
}