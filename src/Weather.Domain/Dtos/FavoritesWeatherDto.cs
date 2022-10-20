﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Weather.Domain.Dtos
{
    public sealed class FavoritesWeatherDto
    {
        public IReadOnlyCollection<CurrentWeatherDto> FavoriteWeathers { get; init; } = new List<CurrentWeatherDto>();
    }
}
