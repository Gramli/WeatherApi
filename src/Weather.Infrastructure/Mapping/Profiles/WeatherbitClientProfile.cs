using AutoMapper;

namespace Weather.Infrastructure.Mapping.Profiles
{
    internal sealed class WeatherbitClientProfile : Profile
    {
        public WeatherbitClientProfile()
        {
            CreateMap<Wheaterbit.Client.Dtos.CurrentWeatherDto, Domain.Dtos.CurrentWeatherDto>()
                .ForMember(dest=>dest.Temperature, opt => opt.MapFrom(src => src.temp))
                .ForMember(dest => dest.DateTime, opt => opt.MapFrom(src => src.ob_time))
                .ForMember(dest => dest.CityName, opt => opt.MapFrom(src => src.city_name))
                .ForMember(dest => dest.Sunrise, opt => opt.MapFrom(src => src.sunrise))
                .ForMember(dest => dest.Sunset, opt => opt.MapFrom(src => src.sunset));

            CreateMap<Wheaterbit.Client.Dtos.ForecastTemperatureDto, Domain.Dtos.ForecastTemperatureDto>()
                .ForMember(dest => dest.Temperature, opt => opt.MapFrom(src => src.temp))
                .ForMember(dest => dest.DateTime, opt => opt.MapFrom(src => src.datetime));

            CreateMap<Wheaterbit.Client.Dtos.ForecastWeatherDto, Domain.Dtos.ForecastWeatherDto>()
                .ForMember(dest => dest.ForecastTemperatures, opt => opt.MapFrom(src => src.Data))
                .ForMember(dest => dest.CityName, opt => opt.MapFrom(src => src.city_name));
        }
    }
}
