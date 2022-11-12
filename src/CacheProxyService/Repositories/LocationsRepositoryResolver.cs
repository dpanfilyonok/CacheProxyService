namespace CacheProxyService.Repositories;

public enum LocationsRepositoryResolverKey
{
    Cache,
    NoCache
}

public delegate ILocationsRepository LocationsRepositoryResolver(LocationsRepositoryResolverKey key);
