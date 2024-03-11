using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Weather.Core.Abstractions;
using Weather.Domain.BusinessEntities;
using Weather.Infrastructure.Database.EFContext;

namespace Weather.Infrastructure.Database.Repositories
{
    internal sealed class WeatherQueriesRepository : RepositoryBase, IWeatherQueriesRepository
    {
        public WeatherQueriesRepository(WeatherContext weatherContext, IMapper mapper)
            : base(weatherContext, mapper) { }

        public async Task<IEnumerable<FavoriteLocation>> GetFavorites(CancellationToken cancellationToken)
        {
            var favoriteLocationEntities = await _weatherContext.FavoriteLocations.ToListAsync(cancellationToken);
            return _mapper.Map<List<FavoriteLocation>>(favoriteLocationEntities);
        }
    }
}
