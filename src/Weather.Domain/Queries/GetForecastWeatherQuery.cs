using Weather.Domain.Dtos;

namespace Weather.Domain.Queries
{
    public sealed class GetForecastWeatherQuery
    {
        public LocationDto Location { get; init; }
        public GetForecastWeatherQuery(long latitude, long longtitude)
        {
            Location = new LocationDto
            {
                Latitude = latitude,
                Longitude = longtitude
            };
        }
    }
}
