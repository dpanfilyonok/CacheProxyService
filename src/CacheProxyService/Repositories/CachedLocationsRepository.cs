using CacheProxyService.Models;
using Newtonsoft.Json;
using StackExchange.Redis;

namespace CacheProxyService.Repositories;

public class CachedLocationsRepository : ILocationsRepository
{
    private readonly ILocationsRepository _inner;
    private readonly ILogger<CachedLocationsRepository> _logger;
    private readonly ConnectionMultiplexer _redis;
    
    // TODO take ILocationsRepository
    public CachedLocationsRepository(LocationsRepositoryResolver resolver, ILogger<CachedLocationsRepository> logger)
    {
        _inner = resolver(LocationsRepositoryResolverKey.NoCache);
        _logger = logger;
        _redis = ConnectionMultiplexer.Connect("localhost:6379");
    }

    public async Task<GeoLocation> GetAsync(GeoCoordinates coords)
    {
        var coordsJson = JsonConvert.SerializeObject(coords);
        
        var db = _redis.GetDatabase();
        string? value = await db.StringGetAsync(coordsJson);
        if (value != null)
        {
            _logger.LogInformation("Getting value from cache");
            return JsonConvert.DeserializeObject<GeoLocation>(value);
        }
        
        var location = await _inner.GetAsync(coords);
        _logger.LogInformation("Setting value to cache");
        db.StringSet(coordsJson, JsonConvert.SerializeObject(location));
        return location;
    }
}