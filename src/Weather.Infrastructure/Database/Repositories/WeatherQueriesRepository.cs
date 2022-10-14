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
        internal WeatherQueriesRepository(WeatherContext weatherContext, IMapper mapper)
        {
            _weatherContext = Guard.Against.Null(weatherContext);
            _mapper = Guard.Against.Null(mapper);
        }
        public async Task<Result<IEnumerable<LocationDto>>> GetFavorites(CancellationToken cancellationToken)
        {
            var facoriteLocationEntities = await _weatherContext.FavoriteLocations.ToListAsync(cancellationToken);
            var resultData = _mapper.Map<List<LocationDto>>(facoriteLocationEntities);

            if(resultData is null)
            {
                return Result.Fail(ErrorMessages.DatabaseGetFailed);
            }

            return Result.Ok((IEnumerable<LocationDto>)resultData);
        }
    }
}
