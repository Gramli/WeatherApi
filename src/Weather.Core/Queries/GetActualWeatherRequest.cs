using Weather.Core.Abstractions;
using Weather.Domain;
using Weather.Domain.Dtos;

namespace Weather.Core.Queries
{
    internal sealed class GetActualWeatherRequest : IGetActualWeatherHandler
    {
        public Task<HttpDataResponse<ActualWeatherDto>> HandleAsync(LocationDto request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
