using Weather.Core.Abstractions;
using Weather.Domain.Dtos;

namespace Weather.Core.Validation
{
    internal sealed class LocationValidator : ILocationValidator
    {
        public bool IsValid(LocationDto locationDto)
        {
            return IsLatitudeValid(locationDto.Latitude) && IsLongitudeValid(locationDto.Longitude);
        }

        private bool IsLatitudeValid(long latitude)
        {
            return latitude >= -90 && latitude <= 90;
        }

        private bool IsLongitudeValid(long longitude)
        {
            return longitude >= -180 && longitude <= 180;
        }
    }
}
