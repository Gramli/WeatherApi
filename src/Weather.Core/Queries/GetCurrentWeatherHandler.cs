using Ardalis.GuardClauses;
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
        private readonly IWeatherService _weatherService;
        internal GetCurrentWeatherHandler(IValidator<LocationDto> locationValidator, IWeatherService weatherService)
        {
            _locationValidator = Guard.Against.Null(locationValidator);
            _weatherService = Guard.Against.Null(weatherService);
        }
        public async Task<HttpDataResponse<CurrentWeatherDto>> HandleAsync(LocationDto request, CancellationToken cancellationToken)
        {
            if(!_locationValidator.IsValid(request))
            {
                return HttpDataResponses.AsBadRequest<CurrentWeatherDto>("Invalid location.");
            }

            var getActualWeatherResult = await _weatherService.GetCurrentWeather(request);
            if(getActualWeatherResult.IsFailed)
            {
                return HttpDataResponses.AsInternalServerError<CurrentWeatherDto>(getActualWeatherResult.Errors.ToErrorMessages().ToArray());
            }

            return HttpDataResponses.AsOK(getActualWeatherResult.Value);
        }
    }
}
