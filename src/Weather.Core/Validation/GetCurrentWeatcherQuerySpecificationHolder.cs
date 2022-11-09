using Validot;
using Weather.Domain.Dtos.Queries;

namespace Weather.Core.Validation
{
    internal sealed class GetCurrentWeatcherQuerySpecificationHolder : ISpecificationHolder<GetCurrentWeatherQuery>
    {
        public Specification<GetCurrentWeatherQuery> Specification { get; }

        public GetCurrentWeatcherQuerySpecificationHolder()
        {
            Specification<GetCurrentWeatherQuery> getCurrentWeatcherQuerySpecification = s => s
                .Member(m => m.Location, GeneralPredicates.isValidLocation);

            Specification = getCurrentWeatcherQuerySpecification;
        }
    }
}
