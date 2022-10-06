using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Weather.Domain.Dtos
{
    public sealed class LocationDto
    {
        public long Latitude { get; init; }
        public long Longitude { get; init; }
    }
}
