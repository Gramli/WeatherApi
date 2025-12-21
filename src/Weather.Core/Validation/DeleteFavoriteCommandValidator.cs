using Validot;
using Weather.Core.Abstractions;
using Weather.Core.HandlerModel;
using Weather.Domain.Commands;

namespace Weather.Core.Validation
{
    internal sealed class DeleteFavoriteCommandValidator : IRequestValidator<DeleteFavoriteCommand>
    {
        private readonly IValidator<DeleteFavoriteCommand> _validator;

        public DeleteFavoriteCommandValidator()
        {
            Specification<DeleteFavoriteCommand> deleteFavoriteCommandSpecification = s => s
            .Member(m => m.Id, r => r.NonNegative());

            _validator = Validot.Validator.Factory.Create(deleteFavoriteCommandSpecification);
        }

        public RequestValidationResult Validate(DeleteFavoriteCommand request)
            => new() { IsValid = _validator.IsValid(request) };
    }
}
