namespace Wheaterbit.Client.Dtos
{
    public class CurrentWeatherDataDto
    {
        public IReadOnlyCollection<CurrentWeatherDto> Data { get; init; }
    }
}
