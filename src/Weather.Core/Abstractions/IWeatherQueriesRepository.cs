using FluentResults;
using Weather.Domain.Dtos;

namespace Weather.Core.Abstractions
{
    public interface IWeatherQueriesRepository
    {
        Task<Result<IEnumerable<LocationDto>>> GetFavorites(CancellationToken cancellationToken);
    }
}
