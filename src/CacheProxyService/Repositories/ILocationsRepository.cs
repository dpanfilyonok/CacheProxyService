using CacheProxyService.Models;

namespace CacheProxyService.Repositories;

public interface ILocationsRepository
{
    Task<GeoLocation> GetAsync(GeoCoordinates coords);
}