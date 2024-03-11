using Ardalis.GuardClauses;
using FluentResults;
using Microsoft.Extensions.Logging;
using Validot;
using Weather.Core.Abstractions;
using Weather.Core.Resources;
using Weather.Domain.BusinessEntities;
using Weather.Domain.Dtos;
using Weather.Domain.Extensions;
using Weather.Domain.Http;
using Weather.Domain.Logging;
using Weather.Domain.Resources;

namespace Weather.Core.Queries
{
    internal sealed class GetFavoritesHandler : IGetFavoritesHandler
    {
        private readonly IValidator<LocationDto> _locationValidator;
        private readonly IValidator<CurrentWeatherDto> _currentWeatherValidator;
        private readonly IWeatherQueriesRepository _weatherQueriesRepository;
        private readonly IWeatherService _weatherService;
        private readonly ILogger<IGetFavoritesHandler> _logger;

        public GetFavoritesHandler(IWeatherQueriesRepository weatherQueriesRepository, 
            IWeatherService weatherService,
            IValidator<LocationDto> locationValidator,
            IValidator<CurrentWeatherDto> currentWeatherValidator,
            ILogger<IGetFavoritesHandler> logger)
        {
            _locationValidator = Guard.Against.Null(locationValidator);
            _currentWeatherValidator = Guard.Against.Null(currentWeatherValidator);
            _weatherQueriesRepository = Guard.Against.Null(weatherQueriesRepository);
            _weatherService = Guard.Against.Null(weatherService);
            _logger = Guard.Against.Null(logger);
        }

        public async Task<HttpDataResponse<FavoritesWeatherDto>> HandleAsync(EmptyRequest request, CancellationToken cancellationToken)
        {
            var favoriteLocationsResult = await _weatherQueriesRepository.GetFavorites(cancellationToken);

            if(!favoriteLocationsResult.HasAny())
            {
                return HttpDataResponses.AsNoContent<FavoritesWeatherDto>();
            }

            return await GetFavoritesAsync(favoriteLocationsResult, cancellationToken);

        }

        private async Task<HttpDataResponse<FavoritesWeatherDto>> GetFavoritesAsync(IEnumerable<FavoriteLocation> favoriteLocationsResult, CancellationToken cancellationToken)
        {
            var result = new List<FavoriteCurrentWeatherDto>();
            var errorMessages = new List<string>();

            await favoriteLocationsResult.ForEachAsync(async (location) =>
            {
                var favoriteWeather = await GetWeatherAsync(location, cancellationToken);

                if(favoriteWeather.IsFailed)
                {
                    errorMessages.AddRange(favoriteWeather.Errors.ToErrorMessages());
                    return;
                }

                result.Add(new FavoriteCurrentWeatherDto
                {
                    CityName = favoriteWeather.Value.CityName,
                    DateTime = favoriteWeather.Value.DateTime,
                    Sunrise = favoriteWeather.Value.Sunrise,
                    Sunset = favoriteWeather.Value.Sunset,
                    Id = location.Id,
                    Temperature = favoriteWeather.Value.Temperature
                });
            });

            return result.Any() ?
                HttpDataResponses.AsOK(new FavoritesWeatherDto { FavoriteWeathers = result, }, errorMessages) :
                HttpDataResponses.AsInternalServerError<FavoritesWeatherDto>(errorMessages);
        }

        private async Task<Result<CurrentWeatherDto>> GetWeatherAsync(LocationDto location, CancellationToken cancellationToken)
        {
            if (!_locationValidator.IsValid(location))
            {
                _logger.LogWarning(LogEvents.FavoriteWeathersGeneral, ErrorLogMessages.InvalidLocation, location);
                return Result.Fail(string.Format(ErrorMessages.InvalidStoredLocation, location));
            }

            var favoriteWeather = await _weatherService.GetCurrentWeather(location, cancellationToken);
            if (favoriteWeather.IsFailed)
            {
                _logger.LogWarning(LogEvents.FavoriteWeathersGeneral, favoriteWeather.Errors.JoinToMessage());
                return Result.Fail(ErrorMessages.ExternalApiError);
            }

            if (!_currentWeatherValidator.IsValid(favoriteWeather.Value))
            {
                _logger.LogWarning(LogEvents.FavoriteWeathersGeneral, ErrorLogMessages.InvalidWeather, location);
                return Result.Fail(ErrorMessages.ExternalApiError);
            }

            return favoriteWeather.Value;
        }
    }
}
