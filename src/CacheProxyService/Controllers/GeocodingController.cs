using CacheProxyService.Filters.ExceptionFilters;
using Microsoft.AspNetCore.Mvc;
using CacheProxyService.Models;
using CacheProxyService.Repositories;

namespace CacheProxyService.Controllers;

[ApiController]
[Route("api/[controller]")]
[TypeFilter(typeof(InvalidOperationExceptionFilter))]
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
    [TypeFilter(typeof(UnableToLocateExceptionFilter))]
    public async Task<IActionResult> GetLocation([FromBody] GeoCoordinates coordinates)
    {
        _logger.LogInformation("GetLocation of {Coordinates}", coordinates);
        var location = await _repo.GetLocationAsync(coordinates);
        _logger.LogInformation("Location is {Location}", location);
        return Ok(location);
    }
}
