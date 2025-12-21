using Validot;
using Weather.Core.Abstractions;
using Weather.Core.HandlerModel;
using Weather.Domain.Dtos;

namespace Weather.Core.Validation
{
    internal sealed class LocationDtoValidator : IRequestValidator<LocationDto>
    {
        private readonly IValidator<LocationDto> _validator;

        public LocationDtoValidator() 
        {
            _validator = Validot.Validator.Factory.Create(GeneralPredicates.isValidLocation);
        }

        public RequestValidationResult Validate(LocationDto request)
            => new() { IsValid = _validator.IsValid(request) };
    }
}
