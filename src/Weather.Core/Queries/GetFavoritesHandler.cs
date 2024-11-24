using Ardalis.GuardClauses;
using FluentResults;
using Microsoft.Extensions.Logging;
using Microsoft.FeatureManagement;
using SmallApiToolkit.Core.Extensions;
using SmallApiToolkit.Core.RequestHandlers;
using SmallApiToolkit.Core.Response;
using SmallApiToolkit.Core.Validation;
using Weather.Core.Abstractions;
using Weather.Core.Resources;
using Weather.Domain.BusinessEntities;
using Weather.Domain.Dtos;
using Weather.Domain.Extensions;
using Weather.Domain.Logging;
using Weather.Domain.Resources;

namespace Weather.Core.Queries
{
    internal sealed class GetFavoritesHandler : IHttpRequestHandler<FavoritesWeatherDto, EmptyRequest>
    {
        private readonly IRequestValidator<LocationDto> _locationValidator;
        private readonly IRequestValidator<CurrentWeatherDto> _currentWeatherValidator;
        private readonly IWeatherQueriesRepository _weatherQueriesRepository;
        private readonly IWeatherService _weatherService;
        private readonly ILogger<GetFavoritesHandler> _logger;

        public GetFavoritesHandler(IWeatherQueriesRepository weatherQueriesRepository, 
            IWeatherService weatherService,
            IRequestValidator<LocationDto> locationValidator,
            IRequestValidator<CurrentWeatherDto> currentWeatherValidator,
            ILogger<GetFavoritesHandler> logger)
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
            if (!_locationValidator.Validate(location).IsValid)
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

            if (!_currentWeatherValidator.Validate(favoriteWeather.Value).IsValid)
            {
                _logger.LogWarning(LogEvents.FavoriteWeathersGeneral, ErrorLogMessages.InvalidWeather, location);
                return Result.Fail(ErrorMessages.ExternalApiError);
            }

            return favoriteWeather.Value;
        }
    }
}
