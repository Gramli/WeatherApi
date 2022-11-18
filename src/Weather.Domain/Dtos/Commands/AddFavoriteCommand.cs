namespace Weather.Domain.Dtos.Commands
{
    public sealed class AddFavoriteCommand
    {
        public LocationDto Location { get; init; } = new LocationDto();
    }
}
