namespace Weather.Domain.Dtos
{
    public sealed class CurrentWeatherDto
    {
        public double Temperature { get; init; }

        public string CityName { get; init; }

        public DateTime DateTime { get; init; }

        public string Sunset { get; init; }

        public string Sunrise { get; init; }
    }
}
