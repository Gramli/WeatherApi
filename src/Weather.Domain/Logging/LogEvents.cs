namespace Weather.Domain.Logging
{
    public static class LogEvents
    {
        public static readonly int GeneralError = 1000;

        //Favorites
        public static readonly int FavoriteWeathersGeneral = 2000;

        public static readonly int FavoriteWeathersStoreToDatabase = 2100;

        public static readonly int FavoriteWeathersGetFromDatabase = 2200;

        //Current
        public static readonly int CurrentWeathersGeneral = 3000;

        public static readonly int CurrentWeathersValidation = 3100;

        public static readonly int CurrentWeathersGet = 3200;

        //Forecast
        public static readonly int ForecastWeathersGeneral = 4000;

        public static readonly int ForecastWeathersValidation = 4100;

        public static readonly int ForecastWeathersGet = 4200;
    }
}
