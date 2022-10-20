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
        private readonly IWeatherService _weatherService;
        internal GetForecastWeatherHandler(IValidator<LocationDto> locationValidator, IWeatherService weatherService)
        {
            _locationValidator = Guard.Against.Null(locationValidator);
            _weatherService = Guard.Against.Null(weatherService);
        }
        public async Task<HttpDataResponse<ForecastWeatherDto>> HandleAsync(LocationDto request, CancellationToken cancellationToken)
        {
            if(!_locationValidator.IsValid(request))
            {
                return HttpDataResponses.AsBadRequest<ForecastWeatherDto>(ErrorMessages.InvalidLocation);
            }

            var forecastResult = await _weatherService.GetForecastWeather(request, cancellationToken);

            if(forecastResult.IsFailed)
            {
                return HttpDataResponses.AsInternalServerError<ForecastWeatherDto>(forecastResult.Errors.ToErrorMessages());
            }

            return HttpDataResponses.AsOK(forecastResult.Value);
        }
    }
}
