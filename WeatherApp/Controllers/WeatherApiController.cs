using Microsoft.AspNetCore.Mvc;
using WeatherApp.Models;
using WeatherApp.Services;

namespace WeatherApp.Controllers;

[ApiController]
[Route("api/weather")]
[Produces("application/json")]
public class WeatherApiController : ControllerBase
{
    private readonly IWeatherService _weather;

    public WeatherApiController(IWeatherService weather)
    {
        _weather = weather;
    }

    /// <summary>Returns a list of all supported NZ cities.</summary>
    [HttpGet("cities")]
    public IActionResult GetCities()
    {
        return Ok(NzCities.All.Select(c => new { c.Name, c.Latitude, c.Longitude }));
    }

    /// <summary>Returns current weather for all NZ cities.</summary>
    [HttpGet("current")]
    public async Task<IActionResult> GetAllCurrent()
    {
        var results = await _weather.GetAllCitiesCurrentWeatherAsync();
        return Ok(results);
    }

    /// <summary>Returns current weather for a specific NZ city.</summary>
    [HttpGet("current/{city}")]
    public async Task<IActionResult> GetCurrent(string city)
    {
        try
        {
            var result = await _weather.GetCurrentWeatherAsync(city);
            return Ok(result);
        }
        catch (ArgumentException ex)
        {
            return NotFound(new { error = ex.Message });
        }
    }

    /// <summary>Returns a 7-day forecast for a specific NZ city.</summary>
    [HttpGet("forecast/{city}")]
    public async Task<IActionResult> GetForecast(string city, [FromQuery] int days = 7)
    {
        if (days < 1 || days > 16)
            return BadRequest(new { error = "days must be between 1 and 16." });

        try
        {
            var result = await _weather.GetForecastAsync(city, days);
            return Ok(result);
        }
        catch (ArgumentException ex)
        {
            return NotFound(new { error = ex.Message });
        }
    }
}
