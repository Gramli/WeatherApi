using Validot;
using Weather.Core.Abstractions;
using Weather.Core.HandlerModel;
using Weather.Domain.Dtos;

namespace Weather.Core.Validation
{
    internal sealed class CurrentWeatherDtoValidator : IRequestValidator<CurrentWeatherDto>
    {
        private readonly IValidator<CurrentWeatherDto> _validator;

        public CurrentWeatherDtoValidator()
        {
            Specification<string> timeStringSpecification = s => s
                .NotEmpty()
                .And()
                .Rule(m => DateTime.TryParse(m, out var _));

            Specification<double> tempSpecification = s => s
                .Rule(GeneralPredicates.isValidTemperature);

            Specification<CurrentWeatherDto> currentWeatherDtoSpecification = s => s
                .Member(m => m.Sunrise, timeStringSpecification)
                .Member(m => m.Sunset, timeStringSpecification)
                .Member(m => m.Temperature, tempSpecification)
                .Member(m => m.CityName, m => m.NotEmpty().NotWhiteSpace());

            _validator = Validot.Validator.Factory.Create(currentWeatherDtoSpecification);
        }

        public RequestValidationResult Validate(CurrentWeatherDto request)
        {
            var result = _validator.Validate(request);
            return new RequestValidationResult
            { 
                IsValid = !result.AnyErrors,
                ErrorMessages = [.. result.Codes]
            };
        }
    }
}
