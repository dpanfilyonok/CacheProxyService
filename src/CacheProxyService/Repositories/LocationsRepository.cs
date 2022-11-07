using CacheProxyService.Models;
using Dadata;

namespace CacheProxyService.Repositories;

public class LocationsRepository : ILocationsRepository
{
    private readonly SuggestClientAsync _api;

    public LocationsRepository()
    {
        // TODO handle null 
        var token = Environment.GetEnvironmentVariable("DADATA_TOKEN");
        _api = new SuggestClientAsync(token);
    }
    
    public async Task<GeoLocation> GetLocationAsync(GeoCoordinates coords)
    {
        var result = await _api.Geolocate(coords.Latitude, coords.Longitude, count: 1);
        return new GeoLocation() {Address = result.suggestions[0].value};
    }
}