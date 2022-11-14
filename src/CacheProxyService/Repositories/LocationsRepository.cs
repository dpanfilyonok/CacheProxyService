using System.Net;
using CacheProxyService.Models;
using CacheProxyService.Models.Exceptions;
using Dadata;
using Dadata.Model;

namespace CacheProxyService.Repositories;

public class LocationsRepository : ILocationsRepository
{
    private readonly ILogger<LocationsRepository> _logger;
    private readonly SuggestClientAsync _api;

    public LocationsRepository(ILogger<LocationsRepository> logger)
    {
        _logger = logger;
        
        const string dadataTokenEnvVariable = "DADATA_TOKEN";
        var token = Environment.GetEnvironmentVariable(dadataTokenEnvVariable);
        if (token == null)
        {
            throw new InvalidOperationException($"Environment variable {dadataTokenEnvVariable} not defined");
        }
        _api = new SuggestClientAsync(token);
    }
    
    public async Task<GeoLocation> GetAsync(GeoCoordinates coords)
    {
        SuggestResponse<Address>? result;
        try
        {
            result = await _api.Geolocate(coords.Latitude, coords.Longitude, count: 1);
        }
        catch (HttpRequestException e) when (e.StatusCode == HttpStatusCode.Unauthorized)
        {
            throw new InvalidOperationException($"Invalid DaData token variable", e);
        }
        
        if (result.suggestions.Count == 0)
        {
            throw new UnableToLocateException(coords);
        }
        
        return new GeoLocation(result.suggestions[0].value);
    }
}