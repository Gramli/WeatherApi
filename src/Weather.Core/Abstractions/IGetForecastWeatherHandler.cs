using Weather.Domain.Dtos;
using Weather.Domain.Queries;

namespace Weather.Core.Abstractions
{
    public interface IGetForecastWeatherHandler : IRequestHandler<ForecastWeatherDto, GetForecastWeatherQuery>
    {
    }
}
