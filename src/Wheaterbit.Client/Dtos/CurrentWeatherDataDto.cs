namespace Wheaterbit.Client.Dtos
{
    public sealed class CurrentWeatherDataDto
    {
        public IReadOnlyCollection<CurrentWeatherDto> Data { get; init; } = new List<CurrentWeatherDto>();
    }
}
