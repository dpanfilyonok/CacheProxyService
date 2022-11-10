using CacheProxyService.Models;

namespace CacheProxyService.Repositories;

public class CachedLocationsRepository : ILocationsRepository
{
    private readonly ILocationsRepository _inner;
    
    public CachedLocationsRepository(ILocationsRepository inner)
    {
        _inner = inner;
    }
    
    public Task<GeoLocation> GetLocationAsync(GeoCoordinates coords)
    {
        throw new NotImplementedException();
    }
}