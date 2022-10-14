using FluentResults;
using Weather.Domain.Dtos;

namespace Weather.Core.Abstractions
{
    public interface IWeatherService
    {
        Task<Result<CurrentWeatherDto>> GetCurrentWeather(LocationDto locationDto, CancellationToken cancellationToken);

        Task<Result<ForecastWeatherDto>> GetForecastWeather(LocationDto locationDto, CancellationToken cancellationToken);
    }
}
