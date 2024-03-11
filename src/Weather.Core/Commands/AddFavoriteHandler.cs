using Ardalis.GuardClauses;
using Microsoft.Extensions.Logging;
using Validot;
using Weather.Core.Abstractions;
using Weather.Core.Resources;
using Weather.Domain.Commands;
using Weather.Domain.Extensions;
using Weather.Domain.Http;
using Weather.Domain.Logging;

namespace Weather.Core.Commands
{
    internal sealed class AddFavoriteHandler : IAddFavoriteHandler
    {
        private readonly IValidator<AddFavoriteCommand> _addFavoriteCommandValidator;
        private readonly ILogger<IAddFavoriteHandler> _logger;
        private readonly IWeatherCommandsRepository _weatherCommandsRepository;
        public AddFavoriteHandler(IWeatherCommandsRepository weatherCommandsRepository, IValidator<AddFavoriteCommand> addFavoriteCommandValidator, ILogger<IAddFavoriteHandler> logger)
        {
            _weatherCommandsRepository = Guard.Against.Null(weatherCommandsRepository);
            _addFavoriteCommandValidator = Guard.Against.Null(addFavoriteCommandValidator);
            _logger = Guard.Against.Null(logger);
        }

        public async Task<HttpDataResponse<int>> HandleAsync(AddFavoriteCommand request, CancellationToken cancellationToken)
        {
            if (!_addFavoriteCommandValidator.IsValid(request))
            {
                return HttpDataResponses.AsBadRequest<int>(string.Format(ErrorMessages.RequestValidationError, request));
            }

            var addResult = await _weatherCommandsRepository.AddFavoriteLocation(request, cancellationToken);
            if(addResult.IsFailed)
            {
                _logger.LogError(LogEvents.FavoriteWeathersStoreToDatabase, addResult.Errors.JoinToMessage());
                return HttpDataResponses.AsInternalServerError<int>(ErrorMessages.CantStoreLocation);
            }

            return HttpDataResponses.AsOK(addResult.Value);
        }
    }
}
