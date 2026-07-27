using MediatR;
using Microsoft.AspNetCore.Mvc;
using RoseApp.API.Authorization;
using RoseApp.Application.Common;
using RoseApp.Application.Features.Blogs.Commands.CreateBlog;
using RoseApp.Application.Features.Blogs.Commands.DeleteBlog;
using RoseApp.Application.Features.Blogs.Commands.UpdateBlog;
using RoseApp.Application.Features.Blogs.Queries.GetAllBlogs;
using RoseApp.Application.Features.Blogs.Queries.GetBlogById;
using RoseApp.Application.Features.Blogs.Queries.GetBlogBySlug;

namespace RoseApp.API.Controllers;

[ApiController]
[Route("api/blogs")]
public class BlogsController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly ICurrentUserService _currentUserService;

    public BlogsController(
        IMediator mediator,
        ICurrentUserService currentUserService)
    {
        _mediator = mediator;
        _currentUserService = currentUserService;
    }

    /// <summary>Get all blogs (paginated)</summary>
    [HttpGet]
    public async Task<IActionResult> GetAll(
        [FromQuery] int page = 1,
        [FromQuery] int limit = 20,
        [FromQuery] string? search = null)
    {
        var result = await _mediator.Send(
            new GetAllBlogsQuery(page, limit, search));

        return StatusCode(result.Code, result);
    }

    /// <summary>Get blog by ID</summary>
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var result = await _mediator.Send(new GetBlogByIdQuery(id));
        return StatusCode(result.Code, result);
    }

    /// <summary>Get blog by slug</summary>
    [HttpGet("slug/{slug}")]
    public async Task<IActionResult> GetBySlug(string slug)
    {
        var result = await _mediator.Send(new GetBlogBySlugQuery(slug));
        return StatusCode(result.Code, result);
    }

    /// <summary>Create a new blog (Admin only)</summary>
    [HttpPost]
    [HasPermission(Permissions.Blogs.Create)]
    public async Task<IActionResult> Create([FromBody] CreateBlogCommand command)
    {
        var result = await _mediator.Send(
            command with { AuthorId = _currentUserService.UserId });
        return StatusCode(result.Code, result);
    }

    /// <summary>Update blog (Admin only)</summary>
    [HttpPatch("{id:guid}")]
    [HasPermission(Permissions.Blogs.Update)]
    public async Task<IActionResult> Update(
        Guid id,
        [FromBody] UpdateBlogCommand command)
    {
        var result = await _mediator.Send(command with { Id = id });
        return StatusCode(result.Code, result);
    }

    /// <summary>Delete blog (Admin only)</summary>
    [HttpDelete("{id:guid}")]
    [HasPermission(Permissions.Blogs.Delete)]
    public async Task<IActionResult> Delete(Guid id)
    {
        var result = await _mediator.Send(new DeleteBlogCommand(id));
        return StatusCode(result.Code, result);
    }
}