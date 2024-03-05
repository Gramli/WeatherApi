using FluentResults;
using Weather.Domain.Dtos;

namespace Weather.Core.Abstractions
{
    public interface IWeatherQueriesRepository
    {
        Task<IEnumerable<LocationDto>> GetFavorites(CancellationToken cancellationToken);
    }
}
