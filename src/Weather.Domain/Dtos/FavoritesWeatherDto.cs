namespace Weather.Domain.Dtos
{
    public sealed class FavoritesWeatherDto
    {
        public IReadOnlyCollection<FavoriteCurrentWeatherDto> FavoriteWeathers { get; init; } = new List<FavoriteCurrentWeatherDto>();
    }
}
