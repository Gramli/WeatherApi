using AutoMapper;
using Weather.Domain.BusinessEntities;
using Weather.Domain.Dtos;
using Weather.Infrastructure.Database.EFContext.Entities;

namespace Weather.Infrastructure.Mapping.Profiles
{
    internal sealed class WeatherEntitiesProfile : Profile
    {
        public WeatherEntitiesProfile() 
        {
            CreateMap<LocationDto, FavoriteLocationEntity>();
            CreateMap<FavoriteLocationEntity, FavoriteLocation>();
        }
    }
}
