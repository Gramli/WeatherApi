using SmallApiToolkit.Core.Validation;
using Validot;
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

        public bool IsValid(DeleteFavoriteCommand request)
            => _validator.IsValid(request);
    }
}
