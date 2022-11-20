using Ardalis.GuardClauses;
using AutoMapper;
using FluentResults;
using Weather.Core.Abstractions;
using Weather.Domain.Dtos.Commands;
using Weather.Infrastructure.Database.EFContext;
using Weather.Infrastructure.Database.EFContext.Entities;

namespace Weather.Infrastructure.Database.Repositories
{
    internal sealed class WeatherCommandsRepository : IWeatherCommandsRepository
    {
        private readonly IMapper _mapper;
        private readonly WeatherContext _weatherContext;
        public WeatherCommandsRepository(WeatherContext weatherContext, IMapper mapper)
        { 
            _weatherContext = Guard.Against.Null(weatherContext);
            _mapper = Guard.Against.Null(mapper);
        }

        public async Task<Result<int>> AddFavoriteLocation(AddFavoriteCommand addFavoriteCommand, CancellationToken cancellationToken)
        {
            var locationEntity = _mapper.Map<FavoriteLocationEntity>(addFavoriteCommand.Location);
            await _weatherContext.FavoriteLocations.AddAsync(locationEntity);
            await _weatherContext.SaveChangesAsync(cancellationToken);
            return Result.Ok(locationEntity.Id);
        }
    }
}
