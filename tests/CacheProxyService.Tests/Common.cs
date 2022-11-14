using CacheProxyService.Models;

namespace CacheProxyService.Tests;

public static class Common
{
    public static readonly GeoCoordinates DefaultCoords = new(55.7546025f, 37.62159949999999f);
    public static readonly GeoLocation DefaultLocation = new("г Москва, Красная пл, д 3");
    
    public static readonly GeoCoordinates DefaultBadCoords = new(0f, 0f);

}