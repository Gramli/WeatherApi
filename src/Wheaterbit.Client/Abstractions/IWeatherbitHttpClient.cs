using FluentResults;
using Wheaterbit.Client.Dtos;

namespace Wheaterbit.Client.Abstractions
{
    public interface IWeatherbitHttpClient
    {
        Task<Result<ForecastWeatherDto>> GetSixteenDayForecast(double latitude, double longitude, CancellationToken cancellationToken);
        Task<Result<CurrentWeatherDataDto>> GetCurrentWeather(double latitude, double longitude, CancellationToken cancellationToken);
    }
}
