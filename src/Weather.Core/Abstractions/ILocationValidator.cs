using Weather.Domain.Dtos;

namespace Weather.Core.Abstractions
{
    internal interface ILocationValidator
    {
        bool IsValid(LocationDto locationDto);
    }
}
