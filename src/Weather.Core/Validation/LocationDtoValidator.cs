using SmallApiToolkit.Core.Validation;
using Validot;
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

        public bool IsValid(LocationDto request)
            => _validator.IsValid(request);
    }
}
