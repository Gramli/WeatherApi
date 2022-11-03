namespace Wheaterbit.Client.Dtos
{
    public sealed class CurrentWeatherDto
    {
        public double temp { get; init; }

        public string city_name { get; init; }

        public DateTime ob_time { get; init; }

        public string sunset { get; init; }

        public string sunrise { get; init; }
    }
}
