using Weather.Domain.Dtos;
using Weather.Domain.Http;

namespace Weather.Core.Abstractions
{
    public interface IGetFavoritesHandler : IRequestHandler<FavoritesWeatherDto, EmptyRequest>
    {
    }
}
