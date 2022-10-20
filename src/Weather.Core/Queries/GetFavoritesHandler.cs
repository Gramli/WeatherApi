using Ardalis.GuardClauses;
using Microsoft.Extensions.Logging;
using Weather.Core.Abstractions;
using Weather.Domain.Dtos;
using Weather.Domain.Extensions;
using Weather.Domain.Http;
using Weather.Domain.Logging;

namespace Weather.Core.Queries
{
    internal sealed class GetFavoritesHandler : IGetFavoritesHandler
    {
        private readonly IWeatherQueriesRepository _weatherQueriesRepository;
        private readonly IWeatherService _weatherService;
        private readonly ILogger<GetFavoritesHandler> _logger;

        public GetFavoritesHandler(IWeatherQueriesRepository weatherQueriesRepository, IWeatherService weatherService, ILogger<GetFavoritesHandler> logger)
        {
            _weatherQueriesRepository = Guard.Against.Null(weatherQueriesRepository);
            _weatherService = Guard.Against.Null(weatherService);
            _logger = Guard.Against.Null(logger);
        }

        public async Task<HttpDataResponse<FavoritesWeatherDto>> HandleAsync(EmptyRequest request, CancellationToken cancellationToken)
        {
            var favoriteLocationsResult = await _weatherQueriesRepository.GetFavorites(cancellationToken);

            if(favoriteLocationsResult.IsFailed)
            {
                return HttpDataResponses.AsInternalServerError<FavoritesWeatherDto>(favoriteLocationsResult.Errors.ToErrorMessages());
            }

            if(!favoriteLocationsResult.Value.HasAny())
            {
                return HttpDataResponses.AsNoContent<FavoritesWeatherDto>();
            }

            var result = new List<CurrentWeatherDto>();
            var errorMessages = new List<string>();

            favoriteLocationsResult.Value.ForEach(async (location) =>
            {
                var favoriteWeather = await _weatherService.GetCurrentWeather(location, cancellationToken);
                if(favoriteWeather.IsFailed)
                {
                    var favoriteWeatherMessages = favoriteWeather.Errors.ToErrorMessages();
                    _logger.LogWarning(LogEvents.FavoriteWeathersGeneral, string.Join(',', favoriteWeatherMessages));
                    errorMessages.AddRange(favoriteWeatherMessages);
                    return;
                }

                result.Add(favoriteWeather.Value);
            });

            return result.Any() ?
                HttpDataResponses.AsOK(new FavoritesWeatherDto{ FavoriteWeathers = result,}, errorMessages) :
                HttpDataResponses.AsInternalServerError<FavoritesWeatherDto>(errorMessages);

        }
    }
}
