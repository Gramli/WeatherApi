using SmallApiToolkit.Core.Validation;
using Validot;
using Weather.Domain.Commands;

namespace Weather.Core.Validation
{
    internal sealed class AddFavoriteCommandValidator : IRequestValidator<AddFavoriteCommand>
    {
        private readonly IValidator<AddFavoriteCommand> _validator;

        public AddFavoriteCommandValidator()
        {
            Specification<AddFavoriteCommand> addFavoriteCommandSpecification = s => s
                .Member(m => m.Location, GeneralPredicates.isValidLocation);

            _validator = Validot.Validator.Factory.Create(addFavoriteCommandSpecification);
        }

        public bool IsValid(AddFavoriteCommand request)
            => _validator.IsValid(request);
        
    }
}
