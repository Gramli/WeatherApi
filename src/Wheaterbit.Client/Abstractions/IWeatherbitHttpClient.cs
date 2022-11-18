using FluentResults;
using Wheaterbit.Client.Dtos;

namespace Wheaterbit.Client.Abstractions
{
    public interface IWeatherbitHttpClient
    {
        Task<Result<ForecastWeatherDto>> GetSixteenDayForecast(long latitude, long longitude, CancellationToken cancellationToken);
        Task<Result<CurrentWeatherDataDto>> GetCurrentWeather(long latitude, long longitude, CancellationToken cancellationToken);
    }
}
