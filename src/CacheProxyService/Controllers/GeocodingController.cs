using CacheProxyService.Filters.ExceptionFilters;
using Microsoft.AspNetCore.Mvc;
using CacheProxyService.Models;
using CacheProxyService.Services;

namespace CacheProxyService.Controllers;

[ApiController]
[Route("api/[controller]")]
public class GeocodingController : ControllerBase
{
    private readonly ILocationService _service;

    public GeocodingController(ILocationService service)
    {
        _service = service;
    }

    [HttpGet("Location")]
    [TypeFilter(typeof(UnableToLocateExceptionFilter))]
    public async Task<IActionResult> GetLocation([FromQuery] GeoCoordinates coordinates)
    {
        var location = await _service.GetLocationAsync(coordinates);
        return Ok(location);
    }
}
