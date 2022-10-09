using Ardalis.GuardClauses;
using Validot;
using Weather.Core.Abstractions;
using Weather.Domain;
using Weather.Domain.Dtos;

namespace Weather.Core.Queries
{
    internal sealed class GetActualWeatherHandler : IGetActualWeatherHandler
    {
        private readonly IValidator<LocationDto> _locationValidator;
        internal GetActualWeatherHandler(IValidator<LocationDto> locationValidator)
        {
            _locationValidator = Guard.Against.Null(locationValidator);
        }
        public Task<HttpDataResponse<ActualWeatherDto>> HandleAsync(LocationDto request, CancellationToken cancellationToken)
        {
            if(!_locationValidator.IsValid(request))
            {

            }

            return null;
        }
    }
}
