using Validot;
using Weather.Domain.Dtos;

namespace Wheaterbit.Client.Validation
{
    internal sealed class CurrentWeatherDtoSpecificationHolder : ISpecificationHolder<CurrentWeatherDto>
    {
        public Specification<CurrentWeatherDto> Specification { get; }

        public CurrentWeatherDtoSpecificationHolder()
        {
            Specification<string> timeStringSpecification = s => s
                .Rule(m => !string.IsNullOrWhiteSpace(m) && DateTime.TryParse(m, out var _));

            Specification<double> tempSpecification = s => s
                .Rule(m => m < 60 && m > -90);

            Specification<string> stringSpecification = s => s
                .Rule(m => !string.IsNullOrWhiteSpace(m));

            Specification<CurrentWeatherDto> currentWeatherDtoSpecification = s => s
            .Member(m => m.Sunrise, timeStringSpecification)
            .Member(m => m.Sunset, timeStringSpecification)
            .Member(m => m.Temperature, tempSpecification)
            .Member(m => m.CityName, stringSpecification);

            Specification = currentWeatherDtoSpecification;
        }
    }
}
