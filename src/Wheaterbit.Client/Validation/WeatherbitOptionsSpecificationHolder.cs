using Validot;
using Wheaterbit.Client.Options;

namespace Wheaterbit.Client.Validation
{
    internal sealed class WeatherbitOptionsSpecificationHolder : ISpecificationHolder<WeatherbitOptions>
    {
        public Specification<WeatherbitOptions> Specification { get; }

        public WeatherbitOptionsSpecificationHolder()
        {
            Specification<string> notEmptyStringSpecification = s => s
                .Rule(m => !string.IsNullOrWhiteSpace(m));

            Specification<WeatherbitOptions> weatherbitOptionsSpecification = s => s
            .Member(m => m.BaseUrl, notEmptyStringSpecification)
            .Member(m => m.XRapidAPIHost, notEmptyStringSpecification)
            .Member(m => m.XRapidAPIKey, notEmptyStringSpecification);

            Specification = weatherbitOptionsSpecification;
        }
    }
}
