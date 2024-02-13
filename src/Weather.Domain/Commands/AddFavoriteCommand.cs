using Weather.Domain.Dtos;

namespace Weather.Domain.Commands
{
    public sealed class AddFavoriteCommand
    {
        public LocationDto Location { get; init; } = new LocationDto();
    }
}
