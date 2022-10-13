using FluentResults;
using Wheaterbit.Client.Dtos;

namespace Wheaterbit.Client.Abstractions
{
    public interface IWheaterbitHttpClient
    {
        Task<Result<ForecastWeatherDto>> GetSixteenDayForecast(double latitude, double longitude);
        Task<Result<CurrentWeatherDataDto>> GetCurrentWeather(double latitude, double longitude);
    }
}
