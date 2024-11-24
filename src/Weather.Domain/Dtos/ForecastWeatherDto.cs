namespace Weather.Domain.Dtos
{
    public sealed class ForecastWeatherDto
    { 
        public IReadOnlyCollection<ForecastTemperatureDto> ForecastTemperatures { get; init; } = [];

        public string CityName { get; init; } = string.Empty;
    }
}
