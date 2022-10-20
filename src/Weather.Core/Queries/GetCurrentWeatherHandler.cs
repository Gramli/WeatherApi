using Ardalis.GuardClauses;
using Microsoft.Extensions.Logging;
using Validot;
using Weather.Core.Abstractions;
using Weather.Domain.Dtos;
using Weather.Domain.Extensions;
using Weather.Domain.Http;

namespace Weather.Core.Queries
{
    internal sealed class GetCurrentWeatherHandler : IGetCurrentWeatherHandler
    {
        private readonly IValidator<LocationDto> _locationValidator;
        private readonly IValidator<CurrentWeatherDto> _currentWeatherValidator;
        private readonly IWeatherService _weatherService;
        private readonly ILogger<GetCurrentWeatherHandler> _logger;
        internal GetCurrentWeatherHandler(IValidator<LocationDto> locationValidator, 
            IValidator<CurrentWeatherDto> currentWeatherValidator, 
            IWeatherService weatherService,
            ILogger<GetCurrentWeatherHandler> logger)
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
                return HttpDataResponses.AsBadRequest<CurrentWeatherDto>(ErrorMessages.InvalidLocation);
            }

            var getActualWeatherResult = await _weatherService.GetCurrentWeather(request, cancellationToken);
            if(getActualWeatherResult.IsFailed)
            {
                return HttpDataResponses.AsInternalServerError<CurrentWeatherDto>(getActualWeatherResult.Errors.ToErrorMessages());
            }

            if(_currentWeatherValidator.IsValid(getActualWeatherResult.Value))
            {
                //TODO complete invalid
                _logger.LogError(/*log error*/)
                return HttpDataResponses.AsInternalServerError<CurrentWeatherDto>(/*Specify message*/);
            }

            return HttpDataResponses.AsOK(getActualWeatherResult.Value);
        }
    }
}
