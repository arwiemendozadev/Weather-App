namespace WeatherApp.Models;

public class CurrentWeather
{
    public string City { get; init; } = string.Empty;
    public double Latitude { get; init; }
    public double Longitude { get; init; }
    public double TemperatureCelsius { get; init; }
    public double ApparentTemperatureCelsius { get; init; }
    public double WindSpeedKph { get; init; }
    public int WindDirection { get; init; }
    public double RelativeHumidity { get; init; }
    public double PrecipitationMm { get; init; }
    public int WeatherCode { get; init; }
    public string WeatherDescription => WeatherCodeToDescription(WeatherCode);
    public string WeatherIcon => WeatherCodeToIcon(WeatherCode);
    public DateTime ObservedAt { get; init; }

    private static string WeatherCodeToDescription(int code) => code switch
    {
        0 => "Clear sky",
        1 => "Mainly clear",
        2 => "Partly cloudy",
        3 => "Overcast",
        45 or 48 => "Fog",
        51 or 53 or 55 => "Drizzle",
        61 or 63 or 65 => "Rain",
        71 or 73 or 75 => "Snow",
        77 => "Snow grains",
        80 or 81 or 82 => "Rain showers",
        85 or 86 => "Snow showers",
        95 => "Thunderstorm",
        96 or 99 => "Thunderstorm with hail",
        _ => "Unknown"
    };

    private static string WeatherCodeToIcon(int code) => code switch
    {
        0 => "☀️",
        1 => "🌤️",
        2 => "⛅",
        3 => "☁️",
        45 or 48 => "🌫️",
        51 or 53 or 55 => "🌦️",
        61 or 63 or 65 => "🌧️",
        71 or 73 or 75 or 77 => "❄️",
        80 or 81 or 82 => "🌧️",
        85 or 86 => "🌨️",
        95 or 96 or 99 => "⛈️",
        _ => "🌡️"
    };
}

public class WeatherForecast
{
    public string City { get; init; } = string.Empty;
    public List<DailyForecast> Daily { get; init; } = [];
}

public class DailyForecast
{
    public DateOnly Date { get; init; }
    public double MaxTemperatureCelsius { get; init; }
    public double MinTemperatureCelsius { get; init; }
    public double PrecipitationSumMm { get; init; }
    public int WeatherCode { get; init; }
    public string WeatherDescription => WeatherCodeToDescription(WeatherCode);
    public string WeatherIcon => WeatherCodeToIcon(WeatherCode);

    private static string WeatherCodeToDescription(int code) => code switch
    {
        0 => "Clear sky",
        1 => "Mainly clear",
        2 => "Partly cloudy",
        3 => "Overcast",
        45 or 48 => "Fog",
        51 or 53 or 55 => "Drizzle",
        61 or 63 or 65 => "Rain",
        71 or 73 or 75 => "Snow",
        77 => "Snow grains",
        80 or 81 or 82 => "Rain showers",
        85 or 86 => "Snow showers",
        95 => "Thunderstorm",
        96 or 99 => "Thunderstorm with hail",
        _ => "Unknown"
    };

    private static string WeatherCodeToIcon(int code) => code switch
    {
        0 => "☀️",
        1 => "🌤️",
        2 => "⛅",
        3 => "☁️",
        45 or 48 => "🌫️",
        51 or 53 or 55 => "🌦️",
        61 or 63 or 65 => "🌧️",
        71 or 73 or 75 or 77 => "❄️",
        80 or 81 or 82 => "🌧️",
        85 or 86 => "🌨️",
        95 or 96 or 99 => "⛈️",
        _ => "🌡️"
    };
}
