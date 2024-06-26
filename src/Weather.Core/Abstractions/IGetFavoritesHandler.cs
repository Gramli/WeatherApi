using SmallApiToolkit.Core.RequestHandlers;
using SmallApiToolkit.Core.Response;
using Weather.Domain.Dtos;

namespace Weather.Core.Abstractions
{
    public interface IGetFavoritesHandler : IHttpRequestHandler<FavoritesWeatherDto, EmptyRequest>
    {
    }
}
