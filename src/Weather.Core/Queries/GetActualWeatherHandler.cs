using Ardalis.GuardClauses;
using Validot;
using Weather.Core.Abstractions;
using Weather.Domain.Dtos;
using Weather.Domain.Extensions;
using Weather.Domain.Http;

namespace Weather.Core.Queries
{
    internal sealed class GetActualWeatherHandler : IGetActualWeatherHandler
    {
        private readonly IValidator<LocationDto> _locationValidator;
        private readonly IWeatherService _weatherService;
        internal GetActualWeatherHandler(IValidator<LocationDto> locationValidator, IWeatherService weatherService)
        {
            _locationValidator = Guard.Against.Null(locationValidator);
            _weatherService = Guard.Against.Null(weatherService);
        }
        public async Task<HttpDataResponse<ActualWeatherDto>> HandleAsync(LocationDto request, CancellationToken cancellationToken)
        {
            if(!_locationValidator.IsValid(request))
            {
                return HttpDataResponses.AsBadRequest<ActualWeatherDto>("Invalid location.");
            }

            var getActualWeatherResult = await _weatherService.GetActualWeather(request);
            if(getActualWeatherResult.IsFailed)
            {
                return HttpDataResponses.AsInternalServerError<ActualWeatherDto>("Failed to get actual Weather.");
            }

            return HttpDataResponses.AsOK(getActualWeatherResult.Value);
        }
    }
}
