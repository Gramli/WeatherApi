using Ardalis.GuardClauses;
using Microsoft.Extensions.Logging;
using Validot;
using Weather.Core.Abstractions;
using Weather.Core.Resources;
using Weather.Domain.Dtos;
using Weather.Domain.Extensions;
using Weather.Domain.Http;
using Weather.Domain.Logging;
using Weather.Domain.Queries;
using Weather.Domain.Resources;

namespace Weather.Core.Queries
{
    internal sealed class GetCurrentWeatherHandler : IGetCurrentWeatherHandler
    {
        private readonly IValidator<GetCurrentWeatherQuery> _getCurrentWeatcherQueryValidator;
        private readonly IValidator<CurrentWeatherDto> _currentWeatherValidator;
        private readonly IWeatherService _weatherService;
        private readonly ILogger<IGetCurrentWeatherHandler> _logger;
        public GetCurrentWeatherHandler(IValidator<GetCurrentWeatherQuery> getCurrentWeatcherQueryValidator, 
            IValidator<CurrentWeatherDto> currentWeatherValidator, 
            IWeatherService weatherService,
            ILogger<IGetCurrentWeatherHandler> logger)
        {
            _getCurrentWeatcherQueryValidator = Guard.Against.Null(getCurrentWeatcherQueryValidator);
            _weatherService = Guard.Against.Null(weatherService);
            _currentWeatherValidator = Guard.Against.Null(currentWeatherValidator);
            _logger = Guard.Against.Null(logger);
        }
        public async Task<HttpDataResponse<CurrentWeatherDto>> HandleAsync(GetCurrentWeatherQuery request, CancellationToken cancellationToken)
        {
            if(!_getCurrentWeatcherQueryValidator.IsValid(request))
            {
                return HttpDataResponses.AsBadRequest<CurrentWeatherDto>(string.Format(ErrorMessages.RequestValidationError, request));
            }

            var getCurrentWeatherResult = await _weatherService.GetCurrentWeather(request.Location, cancellationToken);
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
