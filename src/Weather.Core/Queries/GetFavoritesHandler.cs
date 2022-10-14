using Ardalis.GuardClauses;
using Weather.Core.Abstractions;
using Weather.Domain.Dtos;
using Weather.Domain.Extensions;
using Weather.Domain.Http;

namespace Weather.Core.Queries
{
    internal sealed class GetFavoritesHandler : IGetFavoritesHandler
    {
        private readonly IWeatherQueriesRepository _weatherQueriesRepository;
        private readonly IWeatherService _weatherService;

        public GetFavoritesHandler(IWeatherQueriesRepository weatherQueriesRepository, IWeatherService weatherService)
        {
            _weatherQueriesRepository = Guard.Against.Null(weatherQueriesRepository);
            _weatherService = Guard.Against.Null(weatherService);
        }

        public async Task<HttpDataResponse<FavoritesWeatherDto>> HandleAsync(EmptyRequest request, CancellationToken cancellationToken)
        {
            var favoriteLocationsResult = await _weatherQueriesRepository.GetFavorites(cancellationToken);

            if(favoriteLocationsResult.IsFailed)
            {
                return HttpDataResponses.AsInternalServerError<FavoritesWeatherDto>(favoriteLocationsResult.Errors.ToErrorMessages().ToArray());
            }

            favoriteLocationsResult.Value.ForEach((location) =>
            {
                var favoriteWeather = _weatherService.GetCurrentWeather(location, cancellationToken);
                //...
            });

            throw new NotImplementedException();

        }
    }
}
