namespace Weather.Domain.Dtos
{
    public class CurrentWeatherDto
    {
        public double Temperature { get; init; }

        public string CityName { get; init; } = string.Empty;

        public DateTime DateTime { get; init; }

        public string Sunset { get; init; } = string.Empty;

        public string Sunrise { get; init; } = string.Empty;
    }
}
