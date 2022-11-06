using Ardalis.GuardClauses;
using Validot;
using Weather.Core.Abstractions;
using Weather.Domain.Dtos;
using Weather.Domain.Extensions;
using Weather.Domain.Http;

namespace Weather.Core.Queries
{
    internal sealed class GetForecastWeatherHandler : IGetForecastWeatherHandler
    {
        private readonly IValidator<LocationDto> _locationValidator;
        private readonly IValidator<ForecastWeatherDto> _forecastWeatherValidator;
        private readonly IWeatherService _weatherService;
        public GetForecastWeatherHandler(IValidator<LocationDto> locationValidator, IWeatherService weatherService, IValidator<ForecastWeatherDto> forecastWeatherValidator)
        {
            _locationValidator = Guard.Against.Null(locationValidator);
            _weatherService = Guard.Against.Null(weatherService);
            _forecastWeatherValidator = Guard.Against.Null(forecastWeatherValidator);
        }
        public async Task<HttpDataResponse<ForecastWeatherDto>> HandleAsync(LocationDto request, CancellationToken cancellationToken)
        {
            if(!_locationValidator.IsValid(request))
            {
                return HttpDataResponses.AsBadRequest<ForecastWeatherDto>(string.Format(ErrorMessages.RequestValidationError, request));
            }

            var forecastResult = await _weatherService.GetForecastWeather(request, cancellationToken);

            if(forecastResult.IsFailed)
            {
                return HttpDataResponses.AsInternalServerError<ForecastWeatherDto>(forecastResult.Errors.ToErrorMessages());
            }

            if(!_forecastWeatherValidator.IsValid(forecastResult.Value))
            {
                return HttpDataResponses.AsInternalServerError<ForecastWeatherDto>(ErrorMessages.ExternalApiError);
            }

            return HttpDataResponses.AsOK(forecastResult.Value);
        }
    }
}
