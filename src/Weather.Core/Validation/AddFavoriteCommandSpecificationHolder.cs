using Validot;
using Weather.Domain.Commands;

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
