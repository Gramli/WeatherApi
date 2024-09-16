using Ardalis.GuardClauses;
using Microsoft.Extensions.Logging;
using SmallApiToolkit.Core.Extensions;
using SmallApiToolkit.Core.RequestHandlers;
using SmallApiToolkit.Core.Response;
using SmallApiToolkit.Core.Validation;
using Weather.Core.Abstractions;
using Weather.Core.Resources;
using Weather.Domain.Dtos;
using Weather.Domain.Extensions;
using Weather.Domain.Logging;
using Weather.Domain.Queries;
using Weather.Domain.Resources;

namespace Weather.Core.Queries
{
    internal sealed class GetCurrentWeatherHandler : ValidationHttpRequestHandler<CurrentWeatherDto, GetCurrentWeatherQuery>
    {
        private readonly IRequestValidator<CurrentWeatherDto> _currentWeatherValidator;
        private readonly IWeatherService _weatherService;
        private readonly ILogger<GetCurrentWeatherHandler> _logger;
        public GetCurrentWeatherHandler(IRequestValidator<GetCurrentWeatherQuery> getCurrentWeatherQueryValidator,
            IRequestValidator<CurrentWeatherDto> currentWeatherValidator,
            IWeatherService weatherService,
            ILogger<GetCurrentWeatherHandler> logger)
            : base(getCurrentWeatherQueryValidator)
        {
            _weatherService = Guard.Against.Null(weatherService);
            _currentWeatherValidator = Guard.Against.Null(currentWeatherValidator);
            _logger = Guard.Against.Null(logger);
        }

        protected override HttpDataResponse<CurrentWeatherDto> CreateInvalidResponse(GetCurrentWeatherQuery request, RequestValidationResult validationResult)
        {
            _logger.LogError(LogEvents.CurrentWeathersValidation, validationResult.ToString());
            return HttpDataResponses.AsBadRequest<CurrentWeatherDto>(string.Format(ErrorMessages.RequestValidationError, request));
        }

        protected override async Task<HttpDataResponse<CurrentWeatherDto>> HandleValidRequestAsync(GetCurrentWeatherQuery request, CancellationToken cancellationToken)
        {
            var getCurrentWeatherResult = await _weatherService.GetCurrentWeather(request.Location, cancellationToken);
            if (getCurrentWeatherResult.IsFailed)
            {
                _logger.LogError(LogEvents.CurrentWeathersGet, getCurrentWeatherResult.Errors.JoinToMessage());
                return HttpDataResponses.AsInternalServerError<CurrentWeatherDto>(ErrorMessages.ExternalApiError);
            }

            var validationResult = _currentWeatherValidator.Validate(getCurrentWeatherResult.Value);
            if (!validationResult.IsValid)
            {
                _logger.LogError(LogEvents.CurrentWeathersValidation, ErrorLogMessages.ValidationErrorLog, validationResult.ToString());
                return HttpDataResponses.AsInternalServerError<CurrentWeatherDto>(ErrorMessages.ExternalApiError);
            }

            return HttpDataResponses.AsOK(getCurrentWeatherResult.Value);
        }
    }
}
