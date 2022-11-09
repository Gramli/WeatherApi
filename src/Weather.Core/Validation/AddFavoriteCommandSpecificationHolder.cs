using Validot;
using Weather.Domain.Dtos.Commands;

namespace Weather.Core.Validation
{
    internal sealed class AddFavoriteCommandSpecificationHolder : ISpecificationHolder<AddFavoriteCommand>
    {
        public Specification<AddFavoriteCommand> Specification { get; }

        public AddFavoriteCommandSpecificationHolder()
        {
            Specification<AddFavoriteCommand> addFavoriteCommandSpecification = s => s
                .Member(m => m.Location, GeneralPredicates.isValidLocation);

            Specification = addFavoriteCommandSpecification;
        }
    }
}
