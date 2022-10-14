using Ardalis.GuardClauses;
using FluentResults;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Wheaterbit.Client.Abstractions;
using Wheaterbit.Client.Dtos;
using Wheaterbit.Client.Options;

namespace Wheaterbit.Client
{
    internal sealed class WheaterbitHttpClient : IWheaterbitHttpClient
    {
        private readonly HttpClient _httpClient;
        private readonly IOptions<WeatherbitOptions> _options;

        private const string XRapidAPIKeyHeader = "X-RapidAPI-Key";
        private const string XRapidAPIHostHeader = "X-RapidAPI-Host";
        public WheaterbitHttpClient(IOptions<WeatherbitOptions> options) 
        { 
            _httpClient = new HttpClient();
            _options = Guard.Against.Null(options);
        }

        public async Task<Result<ForecastWeatherDto>> GetSixteenDayForecast(double latitude, double longitude, CancellationToken cancellationToken)
        {
            var request = new HttpRequestMessage()
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri($"{_options.Value.BaseUrl}/current?lon={longitude}&lat={latitude}"),
                Headers = 
                {
                    { XRapidAPIHostHeader, _options.Value.XRapidAPIHost },
                    { XRapidAPIKeyHeader, _options.Value.XRapidAPIKey },
                }
            };

            return await SendAsyncSave<ForecastWeatherDto?>(request, cancellationToken);
        }

        public async Task<Result<CurrentWeatherDataDto>> GetCurrentWeather(double latitude, double longitude, CancellationToken cancellationToken)
        {
            var request = new HttpRequestMessage()
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri($"{_options.Value.BaseUrl}/forecast/3hourly?lat={latitude}&lon={longitude}"),
                Headers =
                {
                    { XRapidAPIHostHeader, _options.Value.XRapidAPIHost },
                    { XRapidAPIKeyHeader, _options.Value.XRapidAPIKey },
                }
            };

            return await SendAsyncSave<CurrentWeatherDataDto?>(request, cancellationToken);
        }

        private async Task<Result<T?>> SendAsyncSave<T>(HttpRequestMessage requestMessage, CancellationToken cancellationToken)
        {
            try
            {
                return await SendAsync<T>(requestMessage, cancellationToken);
            }
            catch (Exception ex)
            {
                return Result.Fail(ex.Message);
            }
        }

        private async Task<Result<T?>> SendAsync<T>(HttpRequestMessage requestMessage, CancellationToken cancellationToken)
        {
            using var response = await _httpClient.SendAsync(requestMessage, cancellationToken);
            if (!response.IsSuccessStatusCode)
            {
                return Result.Fail($"Failed response to {nameof(GetSixteenDayForecast)}");
            }

            var resultContent = await response.Content.ReadAsStringAsync();
            return Result.Ok(JsonConvert.DeserializeObject<T>(resultContent));
        }

    }
}