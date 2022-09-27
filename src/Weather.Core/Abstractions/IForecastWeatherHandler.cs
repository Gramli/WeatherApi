using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Weather.Domain.Dtos;
using Weather.Domain.Payloads;

namespace Weather.Core.Abstractions
{
    public interface IForecastWeatherHandler : IRequestHandler<ForecastWeatherDto, LocationPayload>
    {
    }
}
