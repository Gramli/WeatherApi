using Validot;
using Weather.Domain.Dtos;

namespace Weather.Core.Validation
{
    internal sealed class LocationDtoSpecificationHolder : ISpecificationHolder<LocationDto>
    {
        public Specification<LocationDto> Specification { get; }

        public LocationDtoSpecificationHolder() 
        {
            Specification = GeneralPredicates.isValidLocation;
        }
    }
}
