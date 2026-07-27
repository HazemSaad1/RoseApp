using MediatR;
using Microsoft.AspNetCore.Mvc;
using RoseApp.API.Authorization;
using RoseApp.Application.Common;
using RoseApp.Application.Features.Testimonials.Commands.CreateTestimonial;
using RoseApp.Application.Features.Testimonials.Commands.DeleteTestimonial;
using RoseApp.Application.Features.Testimonials.Commands.UpdateTestimonial;
using RoseApp.Application.Features.Testimonials.Queries.GetAllTestimonials;
using RoseApp.Application.Features.Testimonials.Queries.GetTestimonialById;

namespace RoseApp.API.Controllers;

[ApiController]
[Route("api/testimonials")]
public class TestimonialsController : ControllerBase
{
    private readonly IMediator _mediator;

    public TestimonialsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>Get all testimonials (paginated)</summary>
    [HttpGet]
    public async Task<IActionResult> GetAll(
        [FromQuery] int page = 1,
        [FromQuery] int limit = 20,
        [FromQuery] string? search = null)
    {
        var result = await _mediator.Send(
            new GetAllTestimonialsQuery(page, limit, search));

        return StatusCode(result.Code, result);
    }

    /// <summary>Get testimonial by ID</summary>
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var result = await _mediator.Send(new GetTestimonialByIdQuery(id));
        return StatusCode(result.Code, result);
    }

    /// <summary>Create a testimonial (public, no auth)</summary>
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateTestimonialCommand command)
    {
        var result = await _mediator.Send(command);
        return StatusCode(result.Code, result);
    }

    /// <summary>Update testimonial (Admin only)</summary>
    [HttpPatch("{id:guid}")]
    [HasPermission(Permissions.Testimonials.Update)]
    public async Task<IActionResult> Update(
        Guid id,
        [FromBody] UpdateTestimonialCommand command)
    {
        var result = await _mediator.Send(command with { Id = id });
        return StatusCode(result.Code, result);
    }

    /// <summary>Delete testimonial (Admin only)</summary>
    [HttpDelete("{id:guid}")]
    [HasPermission(Permissions.Testimonials.Delete)]
    public async Task<IActionResult> Delete(Guid id)
    {
        var result = await _mediator.Send(new DeleteTestimonialCommand(id));
        return StatusCode(result.Code, result);
    }
}