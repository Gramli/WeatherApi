using Weather.Domain;
using Weather.Domain.Dtos;

namespace Weather.Core.Abstractions
{
    public interface GetFavoritesHandler : IRequestHandler<FavoritesWeatherDto, EmptyRequest>
    {
    }
}
