using FluentResults;
using Weather.Core.Abstractions;
using Weather.Domain.Dtos;

namespace Weather.Infrastructure.Services
{
    internal sealed class WeatherService : IWeatherService
    {
        public Task<Result<ActualWeatherDto>> GetActualWeather(LocationDto locationDto)
        {
            throw new NotImplementedException();
        }
    }
}
