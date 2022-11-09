namespace Weather.Domain.Dtos.Queries
{
    public class GetForecastWeatherQuery
    {
        public LocationDto Location { get; init; }
        public GetForecastWeatherQuery(long latitude, long longtitude)
        {
            Location = new LocationDto()
            {
                Latitude = latitude,
                Longitude = longtitude
            };
        }
    }
}
