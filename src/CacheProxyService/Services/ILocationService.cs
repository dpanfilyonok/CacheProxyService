using CacheProxyService.Models;
using CacheProxyService.Models.Exceptions;

namespace CacheProxyService.Services;

/// <exception cref="UnableToLocateException">Thrown if coordinates out of range</exception>
/// <exception cref="InvalidOperationException">Thrown if the database is unavailable</exception>
public interface ILocationService
{
    Task<GeoLocation> GetLocationAsync(GeoCoordinates coords);
}