using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CacheProxyService.Models.Exceptions;
using CacheProxyService.Repositories;
using CacheProxyService.Services;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace CacheProxyService.Tests;

public class LocationServiceTests
{
    private readonly Mock<ILocationsRepository> _locationsRepositoryMock = new();
    private readonly Mock<LocationsRepositoryResolver> _resolverMock = new();

    private LocationService _locationService = null!;

    private void EndSetup(LocationsRepositoryResolverKey key)
    {
        _resolverMock
            .Setup(resolver => resolver.Invoke(key))
            .Returns(_locationsRepositoryMock.Object);
        
        _locationService = new LocationService(_resolverMock.Object, Mock.Of<ILogger<LocationService>>(), key);
    }

    [Theory]
    [MemberData(nameof(GetData))]
    public async Task GetLocation(LocationsRepositoryResolverKey key)
    {
        var coords = Common.DefaultCoords;
        var location = Common.DefaultLocation;

        _locationsRepositoryMock
            .Setup(repo => repo.GetAsync(coords))
            .ReturnsAsync(location);

        EndSetup(key);
        
        var actualLocation = await _locationService.GetLocationAsync(coords);
        actualLocation.Should().Be(location);
    }
    
    [Theory]
    [MemberData(nameof(GetData))]
    public async Task GetLocation_UnableToLocateException(LocationsRepositoryResolverKey key)
    {    
        var coords = Common.DefaultBadCoords;

        _locationsRepositoryMock
            .Setup(repo => repo.GetAsync(coords))
            .ThrowsAsync(new UnableToLocateException());
        
        EndSetup(key);

        await _locationService
            .Awaiting(x => x.GetLocationAsync(coords))
            .Should().ThrowAsync<UnableToLocateException>();
    }
    
    [Theory]
    [MemberData(nameof(GetData))]
    public async Task GetLocation_InvalidOperationException(LocationsRepositoryResolverKey key)
    {
        var coords = Common.DefaultBadCoords;

        _locationsRepositoryMock
            .Setup(repo => repo.GetAsync(coords))
            .ThrowsAsync(new InvalidOperationException());
        
        EndSetup(key);

        await _locationService
            .Awaiting(x => x.GetLocationAsync(coords))
            .Should().ThrowAsync<InvalidOperationException>();
    }
    
    public static IEnumerable<object[]> GetData()
    {
        return new List<object[]>
        {
            new object[] { LocationsRepositoryResolverKey.Cache },
            new object[] { LocationsRepositoryResolverKey.NoCache }
        };
    }

}