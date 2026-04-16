# Build stage
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src
COPY WeatherApp/WeatherApp.csproj WeatherApp/
RUN dotnet restore WeatherApp/WeatherApp.csproj
COPY WeatherApp/ WeatherApp/
RUN dotnet publish WeatherApp/WeatherApp.csproj -c Release -o /app/publish --no-restore

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .
ENV ASPNETCORE_URLS=http://+:${PORT:-8080}
EXPOSE 8080
ENTRYPOINT ["dotnet", "WeatherApp.dll"]
