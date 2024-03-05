using Ardalis.GuardClauses;
using AutoMapper;
using FluentResults;
using Microsoft.EntityFrameworkCore;
using Weather.Core.Abstractions;
using Weather.Domain.Dtos;
using Weather.Infrastructure.Database.EFContext;

namespace Weather.Infrastructure.Database.Repositories
{
    internal sealed class WeatherQueriesRepository : RepositoryBase, IWeatherQueriesRepository
    {
        public WeatherQueriesRepository(WeatherContext weatherContext, IMapper mapper)
            : base(weatherContext, mapper) { }

        public async Task<IEnumerable<LocationDto>> GetFavorites(CancellationToken cancellationToken)
        {
            var facoriteLocationEntities = await _weatherContext.FavoriteLocations.ToListAsync(cancellationToken);
            return _mapper.Map<List<LocationDto>>(facoriteLocationEntities);
        }
    }
}
