using Ardalis.GuardClauses;
using AutoMapper;
using Weather.Infrastructure.Database.EFContext;

namespace Weather.Infrastructure.Database
{
    internal abstract class RepositoryBase
    {
        protected readonly IMapper _mapper;
        protected readonly WeatherContext _weatherContext;
        protected RepositoryBase(WeatherContext weatherContext, IMapper mapper)
        {
            _weatherContext = Guard.Against.Null(weatherContext);
            _mapper = Guard.Against.Null(mapper);
        }
    }
}
