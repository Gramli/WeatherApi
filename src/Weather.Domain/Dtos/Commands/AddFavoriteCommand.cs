namespace Weather.Domain.Dtos.Commands
{
    public sealed class AddFavoriteCommand
    {
        public LocationDto Location { get; init; }
        public AddFavoriteCommand(long latitude, long longtitude) 
        {
            Location = new LocationDto()
            {
                Latitude = latitude,
                Longitude = longtitude
            };
        }
    }
}
