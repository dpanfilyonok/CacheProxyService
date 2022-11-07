using Microsoft.AspNetCore.Mvc;
using CacheProxyService.Models;
using CacheProxyService.Repositories;

namespace CacheProxyService.Controllers;

[ApiController]
[Route("api/[controller]")]
public class GeocodingController : ControllerBase
{
    private readonly ILocationsRepository _repo;
    private readonly ILogger<GeocodingController> _logger;

    public GeocodingController(ILocationsRepository repo, ILogger<GeocodingController> logger)
    {
        _repo = repo;
        _logger = logger;
    }

    [HttpPost("GetLocation")]
    public async Task<IActionResult> GetLocation([FromBody] GeoCoordinates coordinates)
    {
        var location = await _repo.GetLocationAsync(coordinates);
        _logger.LogInformation($"aaa");
        return Ok(location);
    }
}
