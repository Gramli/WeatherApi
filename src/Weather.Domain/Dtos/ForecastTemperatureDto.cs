using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Weather.Domain.Dtos
{
    public class ForecastTemperatureDto
    {
        public double Temperature { get; init; }

        public DateTime DateTime { get; init; }
    }
}
