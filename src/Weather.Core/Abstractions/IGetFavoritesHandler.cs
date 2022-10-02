using Weather.Domain;
using Weather.Domain.Dtos;

namespace Weather.Core.Abstractions
{
    public interface IGetFavoritesHandler : IRequestHandler<FavoritesWeatherDto, EmptyRequest>
    {
    }
}
