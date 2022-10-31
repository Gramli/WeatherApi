using Validot;
using Weather.Domain.Dtos;

namespace Weather.Core.Validation
{
    internal sealed class LocationDtoSpecificationHolder : ISpecificationHolder<LocationDto>
    {
        public Specification<LocationDto> Specification { get; }

        internal LocationDtoSpecificationHolder() 
        {
            Specification<long> latitudeSpecification = s => s
                .Rule(m => m >= -90 && m <= 90);

            Specification<long> longtitudeSpecification = s => s
                .Rule(m=> m >= -180 && m <= 180);

            Specification<LocationDto> locationSpecification = s => s
                .Member(m => m.Latitude, latitudeSpecification)
                .Member(m => m.Longitude, longtitudeSpecification);

            Specification = locationSpecification;
        }
    }
}
