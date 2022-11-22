using Ardalis.GuardClauses;
using AutoMapper;
using FluentResults;
using Microsoft.EntityFrameworkCore;
using Weather.Core.Abstractions;
using Weather.Domain.Dtos;
using Weather.Infrastructure.Database.EFContext;

namespace Weather.Infrastructure.Database.Repositories
{
    internal sealed class WeatherQueriesRepository : IWeatherQueriesRepository
    {
        private readonly IMapper _mapper;
        private readonly WeatherContext _weatherContext;
        public WeatherQueriesRepository(WeatherContext weatherContext, IMapper mapper)
        {
            _weatherContext = Guard.Against.Null(weatherContext);
            _mapper = Guard.Against.Null(mapper);
        }
        public async Task<Result<IEnumerable<LocationDto>>> GetFavorites(CancellationToken cancellationToken)
        {
            var facoriteLocationEntities = await _weatherContext.FavoriteLocations.ToListAsync(cancellationToken);
            var resultData = _mapper.Map<List<LocationDto>>(facoriteLocationEntities);
            return Result.Ok((IEnumerable<LocationDto>)resultData);
        }
    }
}
