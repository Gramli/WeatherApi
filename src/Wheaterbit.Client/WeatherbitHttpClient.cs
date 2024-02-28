using Ardalis.GuardClauses;
using FluentResults;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Validot;
using Wheaterbit.Client.Abstractions;
using Wheaterbit.Client.Dtos;
using Wheaterbit.Client.Options;

namespace Wheaterbit.Client
{
    internal sealed class WeatherbitHttpClient : IWeatherbitHttpClient
    {
        private readonly HttpClient _httpClient;
        private readonly IOptions<WeatherbitOptions> _options;
        private readonly IJsonSerializerSettingsFactory _jsonSerializerSettingsFactory;

        private const string XRapidAPIKeyHeader = "X-RapidAPI-Key";
        private const string XRapidAPIHostHeader = "X-RapidAPI-Host";
        public WeatherbitHttpClient(IOptions<WeatherbitOptions> options, 
            IHttpClientFactory httpClientFactory, 
            IValidator<WeatherbitOptions> optionsValidator,
            IJsonSerializerSettingsFactory jsonSerializerSettingsFactory) 
        {
            Guard.Against.Null(httpClientFactory);
            _httpClient = httpClientFactory.CreateClient();
            _options = Guard.Against.Null(options);
            _jsonSerializerSettingsFactory = Guard.Against.Null(jsonSerializerSettingsFactory);

            ValidateOptions(optionsValidator, options);
        }

        private static void ValidateOptions(IValidator<WeatherbitOptions> optionsValidator, IOptions<WeatherbitOptions> options)
        {
            var validationResult = optionsValidator.Validate(options.Value);

            if(validationResult.AnyErrors)
            {
                throw new ArgumentException($"Invalid {nameof(WeatherbitOptions)}: {validationResult}");
            }
        }

        public async Task<Result<ForecastWeatherDto>> GetSixteenDayForecast(double latitude, double longitude, CancellationToken cancellationToken)
        {
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri($"{_options.Value.BaseUrl}/forecast/daily?lon={longitude}&lat={latitude}"),
                Headers = 
                {
                    { XRapidAPIHostHeader, _options.Value.XRapidAPIHost },
                    { XRapidAPIKeyHeader, _options.Value.XRapidAPIKey },
                }
            };

            return await SendAsyncSave<ForecastWeatherDto>(request, cancellationToken);
        }

        public async Task<Result<CurrentWeatherDataDto>> GetCurrentWeather(double latitude, double longitude, CancellationToken cancellationToken)
        {
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri($"{_options.Value.BaseUrl}/current?lat={latitude}&lon={longitude}"),
                Headers =
                {
                    { XRapidAPIHostHeader, _options.Value.XRapidAPIHost },
                    { XRapidAPIKeyHeader, _options.Value.XRapidAPIKey },
                }
            };

            return await SendAsyncSave<CurrentWeatherDataDto>(request, cancellationToken);
        }

        private async Task<Result<T>> SendAsyncSave<T>(HttpRequestMessage requestMessage, CancellationToken cancellationToken)
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

        private async Task<Result<T>> SendAsync<T>(HttpRequestMessage requestMessage, CancellationToken cancellationToken)
        {
            using var response = await _httpClient.SendAsync(requestMessage, cancellationToken);
            if (!response.IsSuccessStatusCode)
            {
                return Result.Fail($"Failed response to {nameof(SendAsync)}");
            }

            var resultContent = await response.Content.ReadAsStringAsync();

            var result = JsonConvert.DeserializeObject<T>(resultContent, _jsonSerializerSettingsFactory.Create());
            if(result is null)
            {
                return Result.Fail($"Failed to deserialize response.");
            }

            return Result.Ok(result);
        }

    }
}