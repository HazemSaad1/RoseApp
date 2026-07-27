using MediatR;
using Microsoft.AspNetCore.Mvc;
using RoseApp.API.Authorization;
using RoseApp.Application.Common;
using RoseApp.Application.Features.SubCategories.Commands.CreateSubCategory;
using RoseApp.Application.Features.SubCategories.Commands.DeleteSubCategory;
using RoseApp.Application.Features.SubCategories.Commands.UpdateSubCategory;
using RoseApp.Application.Features.SubCategories.Queries.GetAllSubCategories;
using RoseApp.Application.Features.SubCategories.Queries.GetSubCategoryById;

namespace RoseApp.API.Controllers;

[ApiController]
[Route("api/sub-categories")]
public class SubCategoriesController : ControllerBase
{
    private readonly IMediator _mediator;

    public SubCategoriesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>Get all sub-categories (paginated)</summary>
    [HttpGet]
    public async Task<IActionResult> GetAll(
        [FromQuery] int page = 1,
        [FromQuery] int limit = 20,
        [FromQuery] string? search = null)
    {
        var result = await _mediator.Send(
            new GetAllSubCategoriesQuery(page, limit, search));

        return StatusCode(result.Code, result);
    }

    /// <summary>Get sub-category by ID</summary>
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var result = await _mediator.Send(new GetSubCategoryByIdQuery(id));
        return StatusCode(result.Code, result);
    }

    /// <summary>Create a new sub-category (Admin only)</summary>
    [HttpPost]
    [HasPermission(Permissions.SubCategories.Create)]
    public async Task<IActionResult> Create([FromBody] CreateSubCategoryCommand command)
    {
        var result = await _mediator.Send(command);
        return StatusCode(result.Code, result);
    }

    /// <summary>Update sub-category (Admin only)</summary>
    [HttpPatch("{id:guid}")]
    [HasPermission(Permissions.SubCategories.Update)]
    public async Task<IActionResult> Update(
        Guid id,
        [FromBody] UpdateSubCategoryCommand command)
    {
        var result = await _mediator.Send(command with { Id = id });
        return StatusCode(result.Code, result);
    }

    /// <summary>Delete sub-category (Admin only)</summary>
    [HttpDelete("{id:guid}")]
    [HasPermission(Permissions.SubCategories.Delete)]
    public async Task<IActionResult> Delete(Guid id)
    {
        var result = await _mediator.Send(new DeleteSubCategoryCommand(id));
        return StatusCode(result.Code, result);
    }
}