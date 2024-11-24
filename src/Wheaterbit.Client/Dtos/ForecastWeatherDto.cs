﻿namespace Wheaterbit.Client.Dtos
{
    public sealed class ForecastWeatherDto
    {
        public IReadOnlyCollection<ForecastTemperatureDto>? Data { get; init; } = [];

        public string? city_name { get; init; } = string.Empty;
    }
}
