using CacheProxyService.Repositories;
using CacheProxyService.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddTransient<ILocationService, LocationService>();
builder.Services.AddSingleton<LocationsRepository>();
builder.Services.AddSingleton<CachedLocationsRepository>();
builder.Services.AddSingleton<LocationsRepositoryResolver>(serviceProvider => key =>
{
    return key switch
    {
        LocationsRepositoryResolverKey.Cache => serviceProvider.GetService<CachedLocationsRepository>(),
        LocationsRepositoryResolverKey.NoCache => serviceProvider.GetService<LocationsRepository>()
    };
});

var app = builder.Build();

// Configure the HTTP request pipeline.

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors(corsBuilder => corsBuilder
    .AllowAnyMethod()
    .AllowAnyHeader()
    .AllowAnyOrigin()
);

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

public partial class Program { }