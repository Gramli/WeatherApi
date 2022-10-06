using Ardalis.GuardClauses;
using AutoMapper;
using Weather.Core.Abstractions;
using Weather.Domain.Dtos;
using Weather.Infrastructure.Database.EFContext;
using Weather.Infrastructure.Database.EFContext.Entities;

namespace Weather.Infrastructure.Database.Repositories
{
    internal sealed class WeatherCommandsRepository : IWeatherCommandsRepository
    {
        private readonly IMapper _mapper;
        private readonly WeatherContext _weatherContext;
        internal WeatherCommandsRepository(WeatherContext weatherContext, IMapper mapper)
        { 
            _weatherContext = Guard.Against.Null(weatherContext);
            _mapper = Guard.Against.Null(mapper);
        }

        public void AddFavoriteLocation(LocationDto locationDto)
        {
            var locationEntity = _mapper.Map<FavoriteLocationEntity>(locationDto);
            _weatherContext.Add(locationEntity);
            _weatherContext.SaveChanges();
        }
    }
}
