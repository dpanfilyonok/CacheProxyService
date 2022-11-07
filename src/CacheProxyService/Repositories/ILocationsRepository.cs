using CacheProxyService.Models;

namespace CacheProxyService.Repositories;

public interface ILocationsRepository
{
    public Task<GeoLocation> GetLocationAsync(GeoCoordinates coords);
}