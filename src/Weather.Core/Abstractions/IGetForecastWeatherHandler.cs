using Weather.Domain.Dtos;
using Weather.Domain.Dtos.Queries;

namespace Weather.Core.Abstractions
{
    public interface IGetForecastWeatherHandler : IRequestHandler<ForecastWeatherDto, GetForecastWeatherQuery>
    {
    }
}
