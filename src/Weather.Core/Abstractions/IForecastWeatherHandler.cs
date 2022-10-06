using Weather.Domain.Dtos;

namespace Weather.Core.Abstractions
{
    public interface IForecastWeatherHandler : IRequestHandler<ForecastWeatherDto, LocationDto>
    {
    }
}
