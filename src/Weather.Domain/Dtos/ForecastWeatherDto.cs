namespace Weather.Domain.Dtos
{
    public sealed class ForecastWeatherDto
    { 
        public IReadOnlyCollection<ForecastTemperatureDto> ForecastTemperatures { get; init; } = new List<ForecastTemperatureDto>();

        public string CityName { get; init; } = string.Empty;
    }
}
