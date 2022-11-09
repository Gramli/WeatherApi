using Validot;
using Weather.Domain.Dtos;

namespace Weather.Core.Validation
{
    internal static class GeneralPredicates
    {
        internal static Predicate<string> isValidString = m => !string.IsNullOrWhiteSpace(m);
        internal static Predicate<double> isValidTemperature = m => m < 60 && m > -90;
        internal static Predicate<long> isValidLatitude = m => m >= -90 && m <= 90;
        internal static Predicate<long> isValidLongtitude = m => m >= -180 && m <= 180;
        internal static Specification<LocationDto> isValidLocation = s => s
                .Member(m => m.Latitude, m => m.Rule(isValidLatitude))
                .Member(m => m.Longitude, m => m.Rule(isValidLongtitude));
    }
}
