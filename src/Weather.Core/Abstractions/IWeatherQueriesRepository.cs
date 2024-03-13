using Weather.Domain.BusinessEntities;

namespace Weather.Core.Abstractions
{
    public interface IWeatherQueriesRepository
    {
        Task<IEnumerable<FavoriteLocation>> GetFavorites(CancellationToken cancellationToken);
    }
}
