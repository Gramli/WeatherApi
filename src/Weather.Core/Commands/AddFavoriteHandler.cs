using Ardalis.GuardClauses;
using Microsoft.Extensions.Logging;
using Weather.Core.Abstractions;
using Weather.Domain;
using Weather.Domain.Dtos;
using Weather.Domain.Extensions;
using Weather.Domain.Http;
using Weather.Domain.Logging;

namespace Weather.Core.Commands
{
    internal sealed class AddFavoriteHandler : IAddFavoriteHandler
    {
        private readonly ILocationValidator _locationValidator;
        private readonly ILogger<IAddFavoriteHandler> _logger;
        private readonly IWeatherCommandsRepository _weatherCommandsRepository;
        internal AddFavoriteHandler(IWeatherCommandsRepository weatherCommandsRepository, ILocationValidator locationValidator, ILogger<IAddFavoriteHandler> logger)
        {
            _weatherCommandsRepository = Guard.Against.Null(weatherCommandsRepository);
            _locationValidator = Guard.Against.Null(locationValidator);
            _logger = Guard.Against.Null(logger);
        }

        public async Task<HttpDataResponse<bool>> HandleAsync(LocationDto request, CancellationToken cancellationToken)
        {
            if (!_locationValidator.IsValid(request))
            {
                return HttpDataResponses.AsBadRequest<bool>(string.Format(ErrorLogMessages.InvalidLocation, request));
            }

            var addResult = await _weatherCommandsRepository.AddFavoriteLocation(request, cancellationToken);
            if(addResult.IsFailed)
            {
                _logger.LogError(LogEvents.FavoriteWeathersStoreToDatabase, addResult.Errors.JoinToMessage());
                return HttpDataResponses.AsInternalServerError<bool>(ErrorMessages.CantStoreLocation);
            }

            return HttpDataResponses.AsOK(true);
        }
    }
}
