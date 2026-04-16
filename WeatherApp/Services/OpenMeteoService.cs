using System.Text.Json;
using Microsoft.Extensions.Caching.Memory;
using WeatherApp.Models;

namespace WeatherApp.Services;

public class OpenMeteoService : IWeatherService
{
    private readonly HttpClient _http;
    private readonly IMemoryCache _cache;
    private static readonly TimeSpan CacheDuration = TimeSpan.FromMinutes(5);
    private const string ForecastBase = "https://api.open-meteo.com/v1/forecast";

    public OpenMeteoService(HttpClient http, IMemoryCache cache)
    {
        _http = http;
        _cache = cache;
    }

    public async Task<CurrentWeather> GetCurrentWeatherAsync(string city)
    {
        var nzCity = NzCities.All.FirstOrDefault(c =>
            c.Name.Equals(city, StringComparison.OrdinalIgnoreCase))
            ?? throw new ArgumentException($"Unknown NZ city: {city}");

        return await FetchCurrentAsync(nzCity);
    }

    public async Task<WeatherForecast> GetForecastAsync(string city, int days = 7)
    {
        var nzCity = NzCities.All.FirstOrDefault(c =>
            c.Name.Equals(city, StringComparison.OrdinalIgnoreCase))
            ?? throw new ArgumentException($"Unknown NZ city: {city}");

        var cacheKey = $"forecast:{nzCity.Name}:{days}";
        if (_cache.TryGetValue(cacheKey, out WeatherForecast? cached))
            return cached!;

        var url = $"{ForecastBase}?latitude={nzCity.Latitude}&longitude={nzCity.Longitude}" +
                  $"&daily=weather_code,temperature_2m_max,temperature_2m_min,precipitation_sum" +
                  $"&timezone=Pacific%2FAuckland&forecast_days={days}";

        var response = await _http.GetAsync(url);
        response.EnsureSuccessStatusCode();

        var json = await response.Content.ReadAsStringAsync();
        using var doc = JsonDocument.Parse(json);
        var daily = doc.RootElement.GetProperty("daily");

        var dates = daily.GetProperty("time").EnumerateArray().Select(e => DateOnly.Parse(e.GetString()!)).ToList();
        var maxTemps = daily.GetProperty("temperature_2m_max").EnumerateArray().Select(e => e.GetDouble()).ToList();
        var minTemps = daily.GetProperty("temperature_2m_min").EnumerateArray().Select(e => e.GetDouble()).ToList();
        var precip = daily.GetProperty("precipitation_sum").EnumerateArray().Select(e => e.GetDouble()).ToList();
        var codes = daily.GetProperty("weather_code").EnumerateArray().Select(e => e.GetInt32()).ToList();

        var forecastDays = dates.Select((date, i) => new DailyForecast
        {
            Date = date,
            MaxTemperatureCelsius = maxTemps[i],
            MinTemperatureCelsius = minTemps[i],
            PrecipitationSumMm = precip[i],
            WeatherCode = codes[i],
        }).ToList();

        var result = new WeatherForecast { City = nzCity.Name, Daily = forecastDays };
        _cache.Set(cacheKey, result, CacheDuration);
        return result;
    }

    public async Task<IEnumerable<CurrentWeather>> GetAllCitiesCurrentWeatherAsync()
    {
        var tasks = NzCities.All.Select(FetchCurrentAsync);
        return await Task.WhenAll(tasks);
    }

    private async Task<CurrentWeather> FetchCurrentAsync(NzCity city)
    {
        var cacheKey = $"current:{city.Name}";
        if (_cache.TryGetValue(cacheKey, out CurrentWeather? cached))
            return cached!;

        var url = $"{ForecastBase}?latitude={city.Latitude}&longitude={city.Longitude}" +
                  "&current=temperature_2m,apparent_temperature,relative_humidity_2m," +
                  "precipitation,weather_code,wind_speed_10m,wind_direction_10m" +
                  "&timezone=Pacific%2FAuckland";

        var response = await _http.GetAsync(url);
        response.EnsureSuccessStatusCode();

        var json = await response.Content.ReadAsStringAsync();
        using var doc = JsonDocument.Parse(json);
        var current = doc.RootElement.GetProperty("current");

        var result = new CurrentWeather
        {
            City = city.Name,
            Latitude = city.Latitude,
            Longitude = city.Longitude,
            TemperatureCelsius = current.GetProperty("temperature_2m").GetDouble(),
            ApparentTemperatureCelsius = current.GetProperty("apparent_temperature").GetDouble(),
            RelativeHumidity = current.GetProperty("relative_humidity_2m").GetDouble(),
            PrecipitationMm = current.GetProperty("precipitation").GetDouble(),
            WeatherCode = current.GetProperty("weather_code").GetInt32(),
            WindSpeedKph = current.GetProperty("wind_speed_10m").GetDouble(),
            WindDirection = current.GetProperty("wind_direction_10m").GetInt32(),
            ObservedAt = DateTime.Parse(current.GetProperty("time").GetString()!),
        };
        _cache.Set(cacheKey, result, CacheDuration);
        return result;
    }
}
