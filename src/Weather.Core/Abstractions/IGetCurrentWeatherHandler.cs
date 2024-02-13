using Weather.Core.Queries;
using Weather.Domain.Dtos;
using Weather.Domain.Queries;

namespace Weather.Core.Abstractions
{
    public interface IGetCurrentWeatherHandler : IRequestHandler<CurrentWeatherDto, GetCurrentWeatherQuery>
    {
    }
}
