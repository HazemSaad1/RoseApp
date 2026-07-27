using MediatR;
using Microsoft.AspNetCore.Mvc;
using RoseApp.API.Authorization;
using RoseApp.Application.Common;
using RoseApp.Application.Features.Categories.Commands.CreateCategory;
using RoseApp.Application.Features.Categories.Commands.DeleteCategory;
using RoseApp.Application.Features.Categories.Commands.UpdateCategory;
using RoseApp.Application.Features.Categories.Queries.GetAllCategories;
using RoseApp.Application.Features.Categories.Queries.GetCategoryById;

namespace RoseApp.API.Controllers;

/// <summary>
/// Category management endpoints
/// </summary>
[ApiController]
[Route("api/categories")]
public class CategoriesController : ControllerBase
{
    private readonly IMediator _mediator;

    public CategoriesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Get all categories (paginated)
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> GetAll(
        [FromQuery] int page = 1,
        [FromQuery] int limit = 20,
        [FromQuery] string? search = null)
    {
        var result = await _mediator.Send(
            new GetAllCategoriesQuery(page, limit, search));

        return StatusCode(result.Code, result);
    }

    /// <summary>
    /// Get category by ID
    /// </summary>
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var result = await _mediator.Send(new GetCategoryByIdQuery(id));
        return StatusCode(result.Code, result);
    }

    /// <summary>
    /// Create a new category (Admin only)
    /// </summary>
    [HttpPost]
    [HasPermission(Permissions.Categories.Create)]
    public async Task<IActionResult> Create([FromBody] CreateCategoryCommand command)
    {
        var result = await _mediator.Send(command);
        return StatusCode(result.Code, result);
    }

    /// <summary>
    /// Update category (Admin only)
    /// </summary>
    [HttpPatch("{id:guid}")]
    [HasPermission(Permissions.Categories.Update)]
    public async Task<IActionResult> Update(
        Guid id,
        [FromBody] UpdateCategoryCommand command)
    {
        var result = await _mediator.Send(command with { Id = id });
        return StatusCode(result.Code, result);
    }

    /// <summary>
    /// Delete category (Admin only)
    /// </summary>
    [HttpDelete("{id:guid}")]
    [HasPermission(Permissions.Categories.Delete)]
    public async Task<IActionResult> Delete(Guid id)
    {
        var result = await _mediator.Send(new DeleteCategoryCommand(id));
        return StatusCode(result.Code, result);
    }
}