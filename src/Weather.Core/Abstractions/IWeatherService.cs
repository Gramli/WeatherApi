using FluentResults;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Weather.Domain.Dtos;

namespace Weather.Core.Abstractions
{
    public interface IWeatherService
    {
        Task<Result<CurrentWeatherDto>> GetCurrentWeather(LocationDto locationDto);
    }
}
