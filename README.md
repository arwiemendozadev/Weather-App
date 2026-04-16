# NZ Weather App

A weather dashboard and REST API for New Zealand cities, built with ASP.NET Core. Live weather data is fetched from [Open-Meteo](https://open-meteo.com) — no API key required.

## Stack

- **ASP.NET Core 10** — MVC (dashboard) + Web API (REST endpoints)
- **Open-Meteo API** — free weather data, no authentication needed
- **Bootstrap 5** — dashboard styling
- **Swashbuckle** — Swagger UI at `/swagger`
- **Docker** — containerised for Render deployment

## Local Setup

**Prerequisites:** [.NET 10 SDK](https://dotnet.microsoft.com/download)

```bash
# Clone the repo
git clone https://github.com/arwiemendozadev/Weather-App.git
cd Weather-App/WeatherApp

# Restore dependencies and run
dotnet run
```

The app will be available at `https://localhost:5001` (or `http://localhost:5000`).

Use `dotnet watch run` instead for hot reload during development.

## API Endpoints

| Method | Route | Description |
|---|---|---|
| GET | `/api/weather/cities` | List all supported NZ cities |
| GET | `/api/weather/current` | Live weather for all cities |
| GET | `/api/weather/current/{city}` | Live weather for a specific city |
| GET | `/api/weather/forecast/{city}?days=7` | Daily forecast (1–16 days) |

Interactive docs available at `/swagger`.
