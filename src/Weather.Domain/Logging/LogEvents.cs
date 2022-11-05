namespace Weather.Domain.Logging
{
    public static class LogEvents
    {
        public const int GeneralError = 1000;

        public const int FavoriteWeathersGeneral = 2000;

        public const int FavoriteWeathersStoreToDatabase = 2100;

        public const int CurrentWeathersGeneral = 3000;

        public const int CurrentWeathersValidation = 3100;
    }
}
