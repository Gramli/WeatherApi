using Validot;
using Weather.Domain.Queries;

namespace Weather.Core.Validation
{
    internal sealed class GetForecastWeatherSpecificationHolder : ISpecificationHolder<GetForecastWeatherQuery>
    {
        public Specification<GetForecastWeatherQuery> Specification { get; }

        public GetForecastWeatherSpecificationHolder()
        {
            Specification<GetForecastWeatherQuery> getForecastWeatherQuerySpecification = s => s
                .Member(m => m.Location, GeneralPredicates.isValidLocation);

            Specification = getForecastWeatherQuerySpecification;
        }
    }
}
