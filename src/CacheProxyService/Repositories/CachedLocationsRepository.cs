using CacheProxyService.Models;
using Newtonsoft.Json;
using StackExchange.Redis;

namespace CacheProxyService.Repositories;

public class CachedLocationsRepository : ILocationsRepository
{
    private readonly ILocationsRepository _inner;
    private readonly ILogger<CachedLocationsRepository> _logger;
    private readonly ConnectionMultiplexer? _redis;
    
    public CachedLocationsRepository(LocationsRepositoryResolver resolver, ILogger<CachedLocationsRepository> logger, LocationsRepositoryResolverKey key)
    {
        _inner = resolver(key);
        _logger = logger;
        try
        {
            _redis = ConnectionMultiplexer.Connect("localhost:6379");
            _logger.LogInformation("Redis connented");
        }
        catch (Exception)
        {
            _redis = null;
            _logger.LogInformation("Cannot connect to redis");
        }
        
    }

    public async Task<GeoLocation> GetAsync(GeoCoordinates coords)
    {
        var coordsJson = JsonConvert.SerializeObject(coords);
        if (_redis == null) return await _inner.GetAsync(coords);
        
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