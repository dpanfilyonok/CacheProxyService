using CacheProxyService.Models;
using CacheProxyService.Repositories;

namespace CacheProxyService.Services;

public class LocationService : ILocationService
{
    private readonly ILocationsRepository _repo;
    private readonly ILogger<LocationService> _logger;

    public LocationService(LocationsRepositoryResolver resolver, ILogger<LocationService> logger)
    {
        _repo = resolver(LocationsRepositoryResolverKey.Cache);
        _logger = logger;
    }
    
    public async Task<GeoLocation> GetLocationAsync(GeoCoordinates coords)
    {
        _logger.LogInformation("GetLocation of {Coordinates}", coords);
        var location = await _repo.GetAsync(coords);
        _logger.LogInformation("Location is {Location}", location);

        return location;
    }
}