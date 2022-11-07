using CacheProxyService.Models;

namespace CacheProxyService.Repositories;

public class CachedLocationsRepository : ILocationsRepository
{
    public Task<GeoLocation> GetLocationAsync(GeoCoordinates coords)
    {
        throw new NotImplementedException();
    }
}