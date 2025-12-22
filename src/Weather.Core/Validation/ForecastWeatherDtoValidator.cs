using Validot;
using Weather.Core.Abstractions;
using Weather.Core.HandlerModel;
using Weather.Domain.Dtos;

namespace Weather.Core.Validation
{
    internal sealed class ForecastWeatherDtoValidator : IRequestValidator<ForecastWeatherDto>
    {
        private readonly IValidator<ForecastWeatherDto> _validator;
        public ForecastWeatherDtoValidator()
        {
            Specification<double> tempSpecification = s => s
                .Rule(GeneralPredicates.isValidTemperature);

            Specification<DateTime> dateTimeSpecification = s => s
                .Rule(s=> s > DateTime.Now.AddDays(-1));

            Specification<ForecastTemperatureDto> forecastTemperatureSpecification = s => s
                .Member(m=>m.Temperature, tempSpecification)
                .Member(m => m.DateTime, dateTimeSpecification);

            Specification<ForecastWeatherDto> forecastSpecification = s => s
                .Member(m => m.ForecastTemperatures, m => m.AsCollection(forecastTemperatureSpecification))
                .Member(m => m.CityName, m=>m.NotEmpty().NotWhiteSpace());

            _validator = Validot.Validator.Factory.Create(forecastSpecification);
        }

        public RequestValidationResult Validate(ForecastWeatherDto request)
            => new() { IsValid = _validator.IsValid(request) };
    }
}
