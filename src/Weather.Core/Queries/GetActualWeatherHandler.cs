using Weather.Core.Abstractions;
using Weather.Domain.Dtos;
using Weather.Domain.Payloads;

namespace Weather.Core.Queries
{
    internal sealed class GetActualWeatherHandler : IGetActualWeatherHandler
    {
        public Task<ActualWeatherDto> HandleAsync(LocationPayload request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
