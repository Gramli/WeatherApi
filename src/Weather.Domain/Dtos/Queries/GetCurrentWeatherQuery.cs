namespace Weather.Domain.Dtos.Queries
{
    public sealed class GetCurrentWeatherQuery
    {
        public LocationDto Location { get; init; }
        public GetCurrentWeatherQuery(long latitude, long longtitude)
        {
            Location = new LocationDto
            {
                Latitude = latitude,
                Longitude = longtitude
            };
        }
    }
}
