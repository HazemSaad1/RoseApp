using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RoseApp.Application.Features.Upload.Commands.UploadImage;

namespace RoseApp.API.Controllers;

[ApiController]
[Route("api/upload")]
[Authorize]
public class UploadController : ControllerBase
{
    private readonly IMediator _mediator;

    public UploadController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>Upload an image (temporary cache)</summary>
    [HttpPost]
    public async Task<IActionResult> Upload(IFormFile file)
    {
        var result = await _mediator.Send(new UploadImageCommand(file));
        return StatusCode(result.Code, result);
    }
}