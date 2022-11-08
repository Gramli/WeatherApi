using Ardalis.GuardClauses;
using Microsoft.Extensions.Logging;
using Validot;
using Weather.Core.Abstractions;
using Weather.Domain;
using Weather.Domain.Dtos;
using Weather.Domain.Extensions;
using Weather.Domain.Http;
using Weather.Domain.Logging;

namespace Weather.Core.Queries
{
    internal sealed class GetCurrentWeatherHandler : IGetCurrentWeatherHandler
    {
        private readonly IValidator<LocationDto> _locationValidator;
        private readonly IValidator<CurrentWeatherDto> _currentWeatherValidator;
        private readonly IWeatherService _weatherService;
        private readonly ILogger<IGetCurrentWeatherHandler> _logger;
        public GetCurrentWeatherHandler(IValidator<LocationDto> locationValidator, 
            IValidator<CurrentWeatherDto> currentWeatherValidator, 
            IWeatherService weatherService,
            ILogger<IGetCurrentWeatherHandler> logger)
        {
            _locationValidator = Guard.Against.Null(locationValidator);
            _weatherService = Guard.Against.Null(weatherService);
            _currentWeatherValidator = Guard.Against.Null(currentWeatherValidator);
            _logger = Guard.Against.Null(logger);
        }
        public async Task<HttpDataResponse<CurrentWeatherDto>> HandleAsync(LocationDto request, CancellationToken cancellationToken)
        {
            if(!_locationValidator.IsValid(request))
            {
                return HttpDataResponses.AsBadRequest<CurrentWeatherDto>(string.Format(ErrorMessages.RequestValidationError, request));
            }

            var getCurrentWeatherResult = await _weatherService.GetCurrentWeather(request, cancellationToken);
            if(getCurrentWeatherResult.IsFailed)
            {
                _logger.LogError(LogEvents.CurrentWeathersGet, getCurrentWeatherResult.Errors.JoinToMessage());
                return HttpDataResponses.AsInternalServerError<CurrentWeatherDto>(ErrorMessages.ExternalApiError);
            }

            var validationResult = _currentWeatherValidator.Validate(getCurrentWeatherResult.Value);
            if(validationResult.AnyErrors)
            {
                _logger.LogError(LogEvents.CurrentWeathersValidation, ErrorLogMessages.ValidationErrorLog, validationResult.ToString());
                return HttpDataResponses.AsInternalServerError<CurrentWeatherDto>(ErrorMessages.ExternalApiError);
            }

            return HttpDataResponses.AsOK(getCurrentWeatherResult.Value);
        }
    }
}
