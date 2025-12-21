using Ardalis.GuardClauses;
using Microsoft.Extensions.Logging;
using Weather.Core.Abstractions;
using Weather.Core.HandlerModel;
using Weather.Core.Resources;
using Weather.Domain.Commands;
using Weather.Domain.Extensions;
using Weather.Domain.Logging;

namespace Weather.Core.Commands
{
    internal sealed class AddFavoriteHandler : ValidationStatusRequestHandler<int, AddFavoriteCommand>
    {
        private readonly ILogger<AddFavoriteHandler> _logger;
        private readonly IWeatherCommandsRepository _weatherCommandsRepository;
        public AddFavoriteHandler(
            IWeatherCommandsRepository weatherCommandsRepository, 
            IRequestValidator<AddFavoriteCommand> addFavoriteCommandValidator, 
            ILogger<AddFavoriteHandler> logger)
            :base(addFavoriteCommandValidator)
        {
            _weatherCommandsRepository = Guard.Against.Null(weatherCommandsRepository);
            _logger = Guard.Against.Null(logger);
        }

        protected override async Task<HandlerResponse<int>> HandleValidRequestAsync(AddFavoriteCommand request, CancellationToken cancellationToken)
        {
            var addResult = await _weatherCommandsRepository.AddFavoriteLocation(request, cancellationToken);
            if(addResult.IsFailed)
            {
                _logger.LogError(LogEvents.FavoriteWeathersStoreToDatabase, addResult.Errors.JoinToMessage());
                return HandlerResponses.AsInternalError<int>(ErrorMessages.CantStoreLocation);
            }

            return HandlerResponses.AsSuccess(addResult.Value);
        }
    }
}
