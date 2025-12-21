using Validot;
using Weather.Core.Abstractions;
using Weather.Core.HandlerModel;
using Weather.Domain.Queries;

namespace Weather.Core.Validation
{
    internal sealed class GetCurrentWeatherQueryValidator : IRequestValidator<GetCurrentWeatherQuery>
    {
        private readonly IValidator<GetCurrentWeatherQuery> _validator;

        public GetCurrentWeatherQueryValidator()
        {
            Specification<GetCurrentWeatherQuery> getCurrentWeatherQuerySpecification = s => s
                .Member(m => m.Location, GeneralPredicates.isValidLocation);

            _validator = Validot.Validator.Factory.Create(getCurrentWeatherQuerySpecification);
        }

        public RequestValidationResult Validate(GetCurrentWeatherQuery request)
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
