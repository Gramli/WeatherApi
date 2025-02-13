using Ardalis.GuardClauses;
using AutoMapper;
using FluentResults;
using Microsoft.Extensions.Options;
using Polly;
using Weather.Core.Abstractions;
using Weather.Domain.Dtos;
using Weather.Domain.Extensions;
using Weather.Infrastructure.Options;
using Weather.Infrastructure.Resources;
using Wheaterbit.Client.Abstractions;

namespace Weather.Infrastructure.Services
{
    internal sealed class WeatherService : IWeatherService
    {
        private readonly IWeatherbitHttpClient _weatherbitHttpClient;
        private readonly IMapper _mapper;
        private readonly IOptionsSnapshot<WeatherServiceRetryPolicyOptions> _optionsSnapshot;

        public WeatherService(IWeatherbitHttpClient weatherbitHttpClient, IMapper mapper, IOptionsSnapshot<WeatherServiceRetryPolicyOptions> optionsSnapshot)
        {
            _weatherbitHttpClient = Guard.Against.Null(weatherbitHttpClient);
            _mapper = Guard.Against.Null(mapper);
            _optionsSnapshot = Guard.Against.Null(optionsSnapshot);
        }

        public async Task<Result<CurrentWeatherDto>> GetCurrentWeather(LocationDto locationDto, CancellationToken cancellationToken)
        {
            var policyResult = await Policy.Handle<Exception>()
                .WaitAndRetryAsync(_optionsSnapshot.Value.RetryCount, _ => TimeSpan.FromSeconds(_optionsSnapshot.Value.Delay))
                .ExecuteAndCaptureAsync<Result<CurrentWeatherDto>>(async () =>
                {
                    var currentWeatherResult = await _weatherbitHttpClient.GetCurrentWeather(locationDto.Latitude, locationDto.Longitude, cancellationToken);
                    if (currentWeatherResult.IsFailed)
                    {
                        return Result.Fail(currentWeatherResult.Errors);
                    }
                    if (currentWeatherResult.Value is null || !currentWeatherResult.Value.Data.HasAny())
                    {
                        return Result.Fail(ErrorMessages.ExternalClientGetDataFailed_EmptyOrNull);
                    }
                    if (currentWeatherResult.Value.Data.Count != 1)
                    {
                        return Result.Fail(string.Format(ErrorMessages.ExternalClientGetDataFailed_CorruptedData_InvalidCount, currentWeatherResult.Value.Data.Count));
                    }
                    return _mapper.Map<CurrentWeatherDto>(currentWeatherResult.Value.Data.Single());
                });

            return HandlePolicyResult(policyResult);
        }

        public async Task<Result<ForecastWeatherDto>> GetFiveDayForecastWeather(LocationDto locationDto, CancellationToken cancellationToken)
            => await GetForecastWeather(_weatherbitHttpClient.GetFiveDayForecast(locationDto.Latitude, locationDto.Longitude, cancellationToken));

        public async Task<Result<ForecastWeatherDto>> GetSixteenDayForecastWeather(LocationDto locationDto, CancellationToken cancellationToken)
            => await GetForecastWeather(_weatherbitHttpClient.GetSixteenDayForecast(locationDto.Latitude, locationDto.Longitude, cancellationToken));

        private async Task<Result<ForecastWeatherDto>> GetForecastWeather(Task<Result<Wheaterbit.Client.Dtos.ForecastWeatherDto>> getData)
        {
            var policyResult = await Policy.Handle<Exception>()
                .WaitAndRetryAsync(_optionsSnapshot.Value.RetryCount, _ => TimeSpan.FromSeconds(_optionsSnapshot.Value.Delay))
                .ExecuteAndCaptureAsync<Result<ForecastWeatherDto>>(async () =>
                {
                    var forecastWeatherResult = await getData;
                    if (forecastWeatherResult.IsFailed)
                    {
                        return Result.Fail(forecastWeatherResult.Errors);
                    }

                    if (forecastWeatherResult.Value is null || !forecastWeatherResult.Value.Data.Any())
                    {
                        return Result.Fail(ErrorMessages.ExternalClientGetDataFailed_EmptyOrNull);
                    }

                    return _mapper.Map<ForecastWeatherDto>(forecastWeatherResult.Value);
                });

            return HandlePolicyResult(policyResult);
        }

        private static Result<T> HandlePolicyResult<T>(PolicyResult<Result<T>> policyResult)
        {
            if (policyResult.Result is null)
            {
                return Result.Fail(ErrorMessages.ExternalClientGetDataFailed)
                    .WithError(policyResult.FinalException.Message);
            }

            return policyResult.Result;
        }
    }
}
