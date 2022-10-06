using Weather.Core.Queries;
using Weather.Domain.Dtos;
using Weather.Domain.Payloads;

namespace Weather.Core.Abstractions
{
    public interface IGetActualWeatherHandler : IRequestHandler<ActualWeatherDto, LocationDto>
    {
    }
}
