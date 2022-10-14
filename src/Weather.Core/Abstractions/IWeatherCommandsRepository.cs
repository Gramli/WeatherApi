using FluentResults;
using Weather.Domain.Dtos;

namespace Weather.Core.Abstractions
{
    public interface IWeatherCommandsRepository
    {
        Task<Result<int>> AddFavoriteLocation(LocationDto locationDto, CancellationToken cancellationToken);
    }
}
