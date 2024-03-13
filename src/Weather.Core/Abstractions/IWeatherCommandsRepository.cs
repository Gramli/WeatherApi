using FluentResults;
using Weather.Domain.Commands;

namespace Weather.Core.Abstractions
{
    public interface IWeatherCommandsRepository
    {
        Task<Result<int>> AddFavoriteLocation(AddFavoriteCommand addFavoriteCommand, CancellationToken cancellationToken);
        Task<Result> DeleteFavoriteLocationSafeAsync(DeleteFavoriteCommand command, CancellationToken cancellationToken);
    }
}
