using WeatherApp.Models;

namespace WeatherApp.Services;

public interface IWeatherService
{
    Task<CurrentWeather> GetCurrentWeatherAsync(string city);
    Task<WeatherForecast> GetForecastAsync(string city, int days = 7);
    Task<IEnumerable<CurrentWeather>> GetAllCitiesCurrentWeatherAsync();
}
