using Ardalis.GuardClauses;
using SmallApiToolkit.Core.Extensions;
using SmallApiToolkit.Core.RequestHandlers;
using SmallApiToolkit.Core.Response;
using SmallApiToolkit.Core.Validation;
using Weather.Core.Abstractions;
using Weather.Domain.Commands;

namespace Weather.Core.Commands
{
    internal sealed class DeleteFavoriteHandler : ValidationHttpRequestHandler<bool, DeleteFavoriteCommand>
    {
        private readonly IWeatherCommandsRepository _weatherCommandsRepository;

        public DeleteFavoriteHandler(
            IWeatherCommandsRepository weatherCommandsRepository,
            IRequestValidator<DeleteFavoriteCommand> validator)
            : base(validator)
        {
            _weatherCommandsRepository = Guard.Against.Null(weatherCommandsRepository);
        }

        protected override async Task<HttpDataResponse<bool>> HandleValidRequestAsync(DeleteFavoriteCommand request, CancellationToken cancellationToken)
        {
            var addResult = await _weatherCommandsRepository.DeleteFavoriteLocationSafeAsync(request, cancellationToken);
            if (addResult.IsFailed)
            {
                return HttpDataResponses.AsInternalServerError<bool>("Location was not deleted from database.");
            }

            return HttpDataResponses.AsOK(true);
        }
    }
}
