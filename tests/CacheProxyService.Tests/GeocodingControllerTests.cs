using System;
using System.Globalization;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using CacheProxyService.Models;
using CacheProxyService.Models.Exceptions;
using CacheProxyService.Services;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Newtonsoft.Json;
using Xunit;
using FluentAssertions;

namespace CacheProxyService.Tests;

// Check the HTTP request is mapped correctly to the controller method.
// Check the ILocationService is called to retrieve location.
// Check the location is serialized correctly to JSON and returned to the caller.
// Check the correct status code.
// Check the filteres.
public class GeocodingControllerTests : IAsyncLifetime
{
    private const string Controller = "Geocoding";
    private const string Action = "Location";
    
    private readonly Mock<ILocationService> _locationServiceMock = new();
    private HttpClient _httpClient = null!;

    public Task InitializeAsync()
    {
        var application = new WebApplicationFactory<Program>()
            .WithWebHostBuilder(builder => builder
                .ConfigureServices(services =>
                {
                    services.AddSingleton(_locationServiceMock.Object);
                })
            );

        _httpClient = application.CreateClient();
        return Task.CompletedTask;
    }

    public Task DisposeAsync()
    {
        return Task.CompletedTask;
    }
    
    [Fact]
    public async Task GetLocation_HappyPath()
    {
        var coords = Common.DefaultCoords;
        var location = Common.DefaultLocation;

        _locationServiceMock
            .Setup(locationService => locationService.GetLocationAsync(coords))
            .ReturnsAsync(location);

        var response = await _httpClient.GetAsync(GetRequestUrl(coords));
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var returnedJson = await response.Content.ReadAsStringAsync();
        var returnedLocation = JsonConvert.DeserializeObject<GeoLocation>(returnedJson);
        returnedLocation.Should().Be(location);
    }
    
    [Fact]
    public async Task GetLocation_UnableToLocateException_404()
    {
        var coords = Common.DefaultBadCoords;

        _locationServiceMock
            .Setup(locationService => locationService.GetLocationAsync(coords))
            .ThrowsAsync(new UnableToLocateException());
        
        var response = await _httpClient.GetAsync(GetRequestUrl(coords));
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }
    
    [Fact]
    public async Task GetLocation_InvalidOperationException_500()
    {
        var coords = Common.DefaultBadCoords;

        _locationServiceMock
            .Setup(locationService => locationService.GetLocationAsync(coords))
            .ThrowsAsync(new InvalidOperationException());
        
        var response = await _httpClient.GetAsync(GetRequestUrl(coords));
        response.StatusCode.Should().Be(HttpStatusCode.InternalServerError);
    }

    private static string GetRequestUrl(GeoCoordinates coords)
    {
        var (latitude, longitude) = coords;
        return $"api/{Controller}/{Action}?Latitude={latitude.ToString(new CultureInfo("en-US"))}&Longitude={longitude.ToString(new CultureInfo("en-US"))}";
    }
}