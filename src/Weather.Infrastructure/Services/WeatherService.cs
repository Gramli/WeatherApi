using Ardalis.GuardClauses;
using AutoMapper;
using FluentResults;
using Weather.Core.Abstractions;
using Weather.Domain.Dtos;
using Wheaterbit.Client.Abstractions;

namespace Weather.Infrastructure.Services
{
    internal sealed class WeatherService : IWeatherService
    {
        private readonly IWheaterbitHttpClient _wheaterbitHttpClient;
        private readonly IMapper _mapper;

        public WeatherService(IWheaterbitHttpClient wheaterbitHttpClient, IMapper mapper)
        {
            _wheaterbitHttpClient = Guard.Against.Null(wheaterbitHttpClient);
            _mapper = Guard.Against.Null(mapper);
        }

        public async Task<Result<CurrentWeatherDto>> GetCurrentWeather(LocationDto locationDto, CancellationToken cancellationToken)
        {
            var currentWeatherResult = await _wheaterbitHttpClient.GetCurrentWeather(locationDto.Latitude, locationDto.Longitude, cancellationToken);
            if(currentWeatherResult.IsFailed)
            {
                return Result.Fail(currentWeatherResult.Errors);
            }

            if(currentWeatherResult.Value.Data is null || !currentWeatherResult.Value.Data.Any())
            {
                return Result.Fail(ErrorMessages.ExternalClientGetDataFailed);
            }

            var currentWeather = currentWeatherResult.Value.Data.SingleOrDefault();

            if(currentWeather is null)
            {
                return Result.Fail(ErrorMessages.ExternalClientGetDataFailed);
            }

            return _mapper.Map<CurrentWeatherDto>(currentWeather);
        }

        public async Task<Result<ForecastWeatherDto>> GetForecastWeather(LocationDto locationDto, CancellationToken cancellationToken)
        {
            var forecastWeatherResult = await _wheaterbitHttpClient.GetSixteenDayForecast(locationDto.Latitude, locationDto.Longitude, cancellationToken);
            if (forecastWeatherResult.IsFailed)
            {
                return Result.Fail(forecastWeatherResult.Errors);
            }

            if (forecastWeatherResult.Value.Data is null || !forecastWeatherResult.Value.Data.Any())
            {
                return Result.Fail(ErrorMessages.ExternalClientGetDataFailed);
            }

            return _mapper.Map<ForecastWeatherDto>(forecastWeatherResult.Value);
        }
    }
}
