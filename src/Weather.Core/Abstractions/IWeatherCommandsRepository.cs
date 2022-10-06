using Weather.Domain.Dtos;

namespace Weather.Core.Abstractions
{
    public interface IWeatherCommandsRepository
    {
        void AddFavoriteLocation(LocationDto locationDto);
    }
}
