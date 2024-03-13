using Ardalis.GuardClauses;
using Validot;
using Weather.Core.Abstractions;
using Weather.Core.Resources;
using Weather.Domain.Commands;
using Weather.Domain.Extensions;
using Weather.Domain.Http;

namespace Weather.Core.Commands
{
    internal sealed class DeleteFavoriteHandler : IDeleteFavoriteHandler
    {
        private readonly IWeatherCommandsRepository _weatherCommandsRepository;
        private readonly IValidator<DeleteFavoriteCommand> _validator;

        public DeleteFavoriteHandler(
            IWeatherCommandsRepository weatherCommandsRepository, 
            IValidator<DeleteFavoriteCommand> validator)
        {
            _weatherCommandsRepository = Guard.Against.Null(weatherCommandsRepository);
            _validator = Guard.Against.Null(validator);
        }

        public async Task<HttpDataResponse<bool>> HandleAsync(DeleteFavoriteCommand request, CancellationToken cancellationToken)
        {
            if (!_validator.IsValid(request))
            {
                return HttpDataResponses.AsBadRequest<bool>(string.Format(ErrorMessages.RequestValidationError, request));
            }

            var addResult = await _weatherCommandsRepository.DeleteFavoriteLocationSafeAsync(request, cancellationToken);
            if (addResult.IsFailed)
            {
                return HttpDataResponses.AsInternalServerError<bool>("Location was not deleted from database.");
            }

            return HttpDataResponses.AsOK(true);
        }
    }
}
