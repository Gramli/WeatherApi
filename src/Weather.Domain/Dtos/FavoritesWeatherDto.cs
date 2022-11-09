namespace Weather.Domain.Dtos
{
    public sealed class FavoritesWeatherDto
    {
        public IReadOnlyCollection<CurrentWeatherDto> FavoriteWeathers { get; init; } = new List<CurrentWeatherDto>();
    }
}
