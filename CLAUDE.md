# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Commands

All commands run from `WeatherApp/`:

```bash
dotnet build                # Build the project
dotnet run                  # Run on https://localhost:5001 / http://localhost:5000
dotnet watch run            # Run with hot reload
dotnet test                 # Run tests (when a test project is added)
```

## Architecture

This is an **ASP.NET Core 10 MVC + Web API** application targeting New Zealand weather data.

### Key layers

| Layer | Path | Responsibility |
|---|---|---|
| Models | `Models/WeatherData.cs`, `Models/NzCity.cs` | Data shapes; `NzCities.All` holds the hardcoded NZ city list |
| Service | `Services/OpenMeteoService.cs` | Calls [Open-Meteo](https://open-meteo.com) (free, no API key); registered as `IWeatherService` via typed `HttpClient` |
| API | `Controllers/WeatherApiController.cs` | REST endpoints under `/api/weather/` |
| Dashboard | `Controllers/HomeController.cs` + `Views/Home/` | MVC views — Index (city grid) and City (detail + 7-day forecast) |

### API endpoints

| Method | Route | Description |
|---|---|---|
| GET | `/api/weather/cities` | List all supported NZ cities |
| GET | `/api/weather/current` | Current weather for all cities |
| GET | `/api/weather/current/{city}` | Current weather for one city |
| GET | `/api/weather/forecast/{city}?days=7` | Daily forecast (1–16 days) |

Swagger UI is available at `/swagger` in all environments.

### Adding a new city

Add an entry to `NzCities.All` in `Models/NzCity.cs` — the rest of the app picks it up automatically (dashboard, API, forecasts).

### Weather codes

`CurrentWeather` and `DailyForecast` both expose `WeatherDescription` and `WeatherIcon` computed from the WMO weather code returned by Open-Meteo. Mapping logic lives in the model classes themselves.
