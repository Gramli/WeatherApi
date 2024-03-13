using Validot;
using Weather.Domain.Commands;

namespace Weather.Core.Validation
{
    internal sealed class DeleteFavoriteCommandSpecificationHolder : ISpecificationHolder<DeleteFavoriteCommand>
    {
        public Specification<DeleteFavoriteCommand> Specification { get; }

        public DeleteFavoriteCommandSpecificationHolder()
        {
            Specification<DeleteFavoriteCommand> deleteFavoriteCommandSpecification = s => s
                .Member(m => m.Id, r => r.NonNegative());

            Specification = deleteFavoriteCommandSpecification;
        }
    }
}
