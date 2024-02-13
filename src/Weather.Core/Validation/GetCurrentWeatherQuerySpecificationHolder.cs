using Validot;
using Weather.Domain.Queries;

namespace Weather.Core.Validation
{
    internal sealed class GetCurrentWeatherQuerySpecificationHolder : ISpecificationHolder<GetCurrentWeatherQuery>
    {
        public Specification<GetCurrentWeatherQuery> Specification { get; }

        public GetCurrentWeatherQuerySpecificationHolder()
        {
            Specification<GetCurrentWeatherQuery> getCurrentWeatherQuerySpecification = s => s
                .Member(m => m.Location, GeneralPredicates.isValidLocation);

            Specification = getCurrentWeatherQuerySpecification;
        }
    }
}
