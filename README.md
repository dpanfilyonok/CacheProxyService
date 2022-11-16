# CacheProxyService

Service providing direct geocoding (from coordinates to address).
An [external API](https://dadata.ru/api/geocode/) is used to determine the address, the results are cached.
Thus this app is a caching proxy service for third-party geocoding service.

## Tech Stack
- **ASP.NET Core** for web api
- **Redis** as in-memory cache
- **xUnit, Moq, FluentAssertions** as testing frameworks
- **Docker** as deployment environment

## Run

    $ export DADATA_TOKEN="<your_dadata_token>"
    $ docker compose up

## REST API

### Get address by given coordinates

**Request**
`GET api/Geocoding/Location`

    curl -i -H 'Accept: application/json' http://localhost/api/Geocoding/Location/?Latitude=55.7546025&Longitude=37.62159949

**Response**

    {
        "address": "г Москва, Красная пл, д 3"
    }