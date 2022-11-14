using CacheProxyService.Repositories;
using CacheProxyService.Services;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace CacheProxyService.Tests;

public class DiTests
{
    [Fact]
    public void AllDependenciesAreRegistered()
    {
        var services = new WebApplicationFactory<Program>().Services;

        services.GetRequiredService<ILocationService>();
        services.GetRequiredService<LocationsRepository>();
        services.GetRequiredService<CachedLocationsRepository>();
        services.GetRequiredService<LocationsRepositoryResolver>();
    }
}