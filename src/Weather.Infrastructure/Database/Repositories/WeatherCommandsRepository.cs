using Ardalis.GuardClauses;
using AutoMapper;
using FluentResults;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Weather.Core.Abstractions;
using Weather.Domain.Commands;
using Weather.Domain.Logging;
using Weather.Infrastructure.Database.EFContext;
using Weather.Infrastructure.Database.EFContext.Entities;

namespace Weather.Infrastructure.Database.Repositories
{
    internal sealed class WeatherCommandsRepository : RepositoryBase, IWeatherCommandsRepository
    {
        private readonly ILogger<IWeatherCommandsRepository> _logger;
        public WeatherCommandsRepository(WeatherContext weatherContext, IMapper mapper, ILogger<IWeatherCommandsRepository> logger)
            : base(weatherContext, mapper) 
        {
            _logger = Guard.Against.Null(logger);
        }

        public async Task<Result<int>> AddFavoriteLocation(AddFavoriteCommand addFavoriteCommand, CancellationToken cancellationToken)
        {
            var locationEntity = _mapper.Map<FavoriteLocationEntity>(addFavoriteCommand.Location);
            try
            {
                await _weatherContext.FavoriteLocations.AddAsync(locationEntity);
                await _weatherContext.SaveChangesAsync(cancellationToken);
                return Result.Ok(locationEntity.Id);
            }
            catch(DbUpdateException ex)
            {
                _logger.LogError(LogEvents.FavoriteWeathersStoreToDatabase, ex, "Can't add favorite locations into database.");
                return Result.Fail(ex.Message);
            }
        }

        public async Task<Result> DeleteFavoriteLocationSafeAsync(DeleteFavoriteCommand command, CancellationToken cancellationToken)
        {
            try
            {
                var location = await _weatherContext.FavoriteLocations.FindAsync(command.Id, cancellationToken);
                _weatherContext.FavoriteLocations.Remove(location!);
                await _weatherContext.SaveChangesAsync(cancellationToken);
                return Result.Ok();
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(LogEvents.FavoriteWeathersStoreToDatabase, ex, "Can't delete location.");
                return Result.Fail(ex.Message);
            }
        }
    }
}
