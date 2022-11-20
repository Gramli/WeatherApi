using FluentResults;
using Weather.Domain.Dtos.Commands;

namespace Weather.Core.Abstractions
{
    public interface IWeatherCommandsRepository
    {
        Task<Result<int>> AddFavoriteLocation(AddFavoriteCommand addFavoriteCommand, CancellationToken cancellationToken);
    }
}
