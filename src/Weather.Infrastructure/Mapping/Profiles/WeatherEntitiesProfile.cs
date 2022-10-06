using AutoMapper;
using Weather.Domain.Dtos;
using Weather.Infrastructure.Database.EFContext.Entities;

namespace Weather.Infrastructure.Mapping.Profiles
{
    internal class WeatherEntitiesProfile : Profile
    {
        internal WeatherEntitiesProfile() 
        {
            CreateMap<LocationDto, FavoriteLocationEntity>();
            CreateMap<FavoriteLocationEntity, LocationDto>();
        }
    }
}
