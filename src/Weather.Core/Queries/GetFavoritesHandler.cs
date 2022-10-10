using Weather.Core.Abstractions;
using Weather.Domain.Dtos;
using Weather.Domain.Http;

namespace Weather.Core.Queries
{
    internal sealed class GetFavoritesHandler : IGetFavoritesHandler
    {
        public Task<HttpDataResponse<FavoritesWeatherDto>> HandleAsync(EmptyRequest request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
