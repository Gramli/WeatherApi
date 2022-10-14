using Weather.Domain.Dtos;

namespace Weather.Core.Abstractions
{
    public interface IGetForecastWeatherHandler : IRequestHandler<ForecastWeatherDto, LocationDto>
    {
    }
}
