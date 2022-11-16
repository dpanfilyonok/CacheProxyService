FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /source
COPY ["src/CacheProxyService/CacheProxyService.csproj", "src/CacheProxyService/"]
RUN dotnet restore "src/CacheProxyService/CacheProxyService.csproj"
COPY . .
WORKDIR "/source/src/CacheProxyService"
RUN dotnet build "CacheProxyService.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "CacheProxyService.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "CacheProxyService.dll"]
