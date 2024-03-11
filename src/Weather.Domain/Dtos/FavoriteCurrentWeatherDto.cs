namespace Weather.Domain.Dtos
{
    public sealed class FavoriteCurrentWeatherDto : CurrentWeatherDto
    {
        public int Id { get; init; }
    }
}
