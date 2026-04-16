namespace WeatherApp.Models;

public static class NzCities
{
    public static readonly IReadOnlyList<NzCity> All = new List<NzCity>
    {
        new("Auckland",      -36.8485, 174.7633),
        new("Wellington",    -41.2866, 174.7756),
        new("Christchurch",  -43.5321, 172.6362),
        new("Hamilton",      -37.7870, 175.2793),
        new("Tauranga",      -37.6878, 176.1651),
        new("Dunedin",       -45.8742, 170.5036),
        new("Queenstown",    -45.0312, 168.6626),
        new("Napier",        -39.4928, 176.9120),
    };
}

public record NzCity(string Name, double Latitude, double Longitude);
