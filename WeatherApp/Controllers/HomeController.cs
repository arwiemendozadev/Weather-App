using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using WeatherApp.Models;
using WeatherApp.Services;

namespace WeatherApp.Controllers;

public class HomeController : Controller
{
    private readonly IWeatherService _weather;

    public HomeController(IWeatherService weather)
    {
        _weather = weather;
    }

    public async Task<IActionResult> Index()
    {
        var cities = await _weather.GetAllCitiesCurrentWeatherAsync();
        return View(cities);
    }

    public async Task<IActionResult> City(string name, int days = 7)
    {
        try
        {
            var current = await _weather.GetCurrentWeatherAsync(name);
            var forecast = await _weather.GetForecastAsync(name, days);
            ViewBag.Forecast = forecast;
            return View(current);
        }
        catch (ArgumentException)
        {
            return NotFound();
        }
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
