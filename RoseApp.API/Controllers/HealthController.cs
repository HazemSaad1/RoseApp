using Microsoft.AspNetCore.Mvc;

namespace RoseApp.API.Controllers;

[ApiController]
[Route("health")]
public class HealthController : ControllerBase
{
    /// <summary>Health check endpoint</summary>
    [HttpGet]
    public IActionResult Check()
        => Ok(new
        {
            status = "healthy",
            timestamp = DateTime.UtcNow
        });
}