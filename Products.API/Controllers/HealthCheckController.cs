using Microsoft.AspNetCore.Mvc;

namespace Products.API.Controllers;


/// <summary>
/// Controller for health check endpoints.
/// </summary>
[Route("api/[controller]")]
[ApiController]
public class HealthCheckController : ControllerBase
{
    /// <summary>
    /// Gets the health status of the API.
    /// </summary>
    /// <returns>An IActionResult containing the health status and timestamp.</returns>
    [HttpGet("ok")]
    public IActionResult Get()
    {
        return Ok(new { status = "Healthy", timestamp = DateTime.UtcNow });
    }
}
