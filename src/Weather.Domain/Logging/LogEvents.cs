namespace Weather.Domain.Logging
{
    public static class LogEvents
    {
        public const int GeneralError = 1000;

        //Favorites
        public const int FavoriteWeathersGeneral = 2000;

        public const int FavoriteWeathersStoreToDatabase = 2100;

        public const int FavoriteWeathersGetFromDatabase = 2200;

        //Current
        public const int CurrentWeathersGeneral = 3000;

        public const int CurrentWeathersValidation = 3100;

        public const int CurrentWeathersGet = 3200;

        //Forecast
        public const int ForecastWeathersGeneral = 4000;

        public const int ForecastWeathersValidation = 4100;

        public const int ForecastWeathersGet = 4200;
    }
}
