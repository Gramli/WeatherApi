using Ardalis.GuardClauses;
using Weather.Core.Abstractions;
using Weather.Core.HandlerModel;
using Weather.Domain.Commands;

namespace Weather.Core.Commands
{
    internal sealed class DeleteFavoriteHandler : ValidationCoreRequestHandler<bool, DeleteFavoriteCommand>
    {
        private readonly IWeatherCommandsRepository _weatherCommandsRepository;

        public DeleteFavoriteHandler(
            IWeatherCommandsRepository weatherCommandsRepository,
            IRequestValidator<DeleteFavoriteCommand> validator)
            : base(validator)
        {
            _weatherCommandsRepository = Guard.Against.Null(weatherCommandsRepository);
        }

        protected override async Task<HandlerResponse<bool>> HandleValidRequestAsync(DeleteFavoriteCommand request, CancellationToken cancellationToken)
        {
            var addResult = await _weatherCommandsRepository.DeleteFavoriteLocationSafeAsync(request, cancellationToken);
            if (addResult.IsFailed)
            {
                return HandlerResponses.AsInternalError<bool>("Location was not deleted from database.");
            }

            return HandlerResponses.AsSuccess(true);
        }
    }
}
