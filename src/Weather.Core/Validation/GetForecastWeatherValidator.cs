using SmallApiToolkit.Core.Validation;
using Validot;
using Weather.Domain.Queries;

namespace Weather.Core.Validation
{
    internal sealed class GetForecastWeatherValidator : IRequestValidator<GetForecastWeatherQuery>
    {
        private readonly IValidator<GetForecastWeatherQuery> _validator;

        public GetForecastWeatherValidator()
        {
            Specification<GetForecastWeatherQuery> getForecastWeatherQuerySpecification = s => s
                .Member(m => m.Location, GeneralPredicates.isValidLocation);

            _validator = Validot.Validator.Factory.Create(getForecastWeatherQuerySpecification);
        }

        public bool IsValid(GetForecastWeatherQuery request)
            => _validator.IsValid(request);
    }
}
