using Ardalis.GuardClauses;
using Microsoft.Extensions.Logging;
using SmallApiToolkit.Core.Extensions;
using SmallApiToolkit.Core.Response;
using SmallApiToolkit.Core.Validation;
using Weather.Core.Abstractions;
using Weather.Core.Resources;
using Weather.Domain.Dtos;
using Weather.Domain.Extensions;
using Weather.Domain.Logging;
using Weather.Domain.Queries;
using Weather.Domain.Resources;

namespace Weather.Core.Queries
{
    internal sealed class GetForecastWeatherHandler : IGetForecastWeatherHandler
    {
        private readonly IRequestValidator<GetForecastWeatherQuery> _getForecastWeatherQueryValidator;
        private readonly IRequestValidator<ForecastWeatherDto> _forecastWeatherValidator;
        private readonly IWeatherService _weatherService;
        private readonly ILogger<IGetForecastWeatherHandler> _logger;
        public GetForecastWeatherHandler(
            IRequestValidator<GetForecastWeatherQuery> getForecastWeatherQueryValidator, 
            IWeatherService weatherService,
            IRequestValidator<ForecastWeatherDto> forecastWeatherValidator,
            ILogger<IGetForecastWeatherHandler> logger)
        {
            _getForecastWeatherQueryValidator = Guard.Against.Null(getForecastWeatherQueryValidator);
            _weatherService = Guard.Against.Null(weatherService);
            _forecastWeatherValidator = Guard.Against.Null(forecastWeatherValidator);
            _logger = Guard.Against.Null(logger);
        }
        public async Task<HttpDataResponse<ForecastWeatherDto>> HandleAsync(GetForecastWeatherQuery request, CancellationToken cancellationToken)
        {
            if(!_getForecastWeatherQueryValidator.IsValid(request))
            {
                return HttpDataResponses.AsBadRequest<ForecastWeatherDto>(string.Format(ErrorMessages.RequestValidationError, request));
            }

            var forecastResult = await _weatherService.GetForecastWeather(request.Location, cancellationToken);

            if(forecastResult.IsFailed)
            {
                _logger.LogError(LogEvents.ForecastWeathersGet, forecastResult.Errors.JoinToMessage());
                return HttpDataResponses.AsInternalServerError<ForecastWeatherDto>(ErrorMessages.ExternalApiError);
            }

            var validationResult = _forecastWeatherValidator.Validate(forecastResult.Value);
            if (validationResult.AnyErrors)
            {
                _logger.LogError(LogEvents.ForecastWeathersValidation, ErrorLogMessages.ValidationErrorLog, validationResult.ToString());
                return HttpDataResponses.AsInternalServerError<ForecastWeatherDto>(ErrorMessages.ExternalApiError);
            }

            return HttpDataResponses.AsOK(forecastResult.Value);
        }
    }
}
