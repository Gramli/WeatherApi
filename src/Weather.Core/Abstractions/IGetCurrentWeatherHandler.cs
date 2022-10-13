using Weather.Core.Queries;
using Weather.Domain.Dtos;

namespace Weather.Core.Abstractions
{
    public interface IGetCurrentWeatherHandler : IRequestHandler<CurrentWeatherDto, LocationDto>
    {
    }
}
