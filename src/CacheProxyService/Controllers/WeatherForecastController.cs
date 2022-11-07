using Microsoft.AspNetCore.Mvc;
using CacheProxyService.Models;

namespace CacheProxyService.Controllers;

[ApiController]
[Route("api/[controller]")]
public class GeocodingController : ControllerBase
{
    private readonly ILogger<GeocodingController> _logger;

    public GeocodingController(ILogger<GeocodingController> logger)
    {
        _logger = logger;
    }

    [HttpPost("getLocation")]
    public IActionResult GetLocation([FromBody] GeoCoordinates coordinates)
    {
        _logger.LogInformation($"aaa");
        return Ok("aaa");
    }
}
