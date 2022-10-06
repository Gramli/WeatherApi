using Ardalis.GuardClauses;
using Weather.Core.Abstractions;
using Weather.Domain;
using Weather.Domain.Dtos;

namespace Weather.Core.Queries
{
    internal sealed class GetActualWeatherHandler : IGetActualWeatherHandler
    {
        private readonly ILocationValidator _locationValidator;
        internal GetActualWeatherHandler(ILocationValidator locationValidator)
        {
            _locationValidator = Guard.Against.Null(locationValidator);
        }
        public Task<HttpDataResponse<ActualWeatherDto>> HandleAsync(LocationDto request, CancellationToken cancellationToken)
        {
            if(!_locationValidator.IsValid(request))
            {

            }
        }
    }
}
