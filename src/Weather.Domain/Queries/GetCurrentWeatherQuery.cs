using Weather.Domain.Dtos;

namespace Weather.Domain.Queries
{
    public sealed class GetCurrentWeatherQuery
    {
        public LocationDto Location { get; init; }
        public GetCurrentWeatherQuery(double latitude, double longtitude)
        {
            Location = new LocationDto
            {
                Latitude = latitude,
                Longitude = longtitude
            };
        }
    }
}
