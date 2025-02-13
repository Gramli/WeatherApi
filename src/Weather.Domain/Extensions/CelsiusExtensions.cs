namespace Weather.Domain.Extensions
{
    public static class CelsiusExtensions
    {
        private const double FahrenheitCelsiusConst = 33.8;
        public static double ToCelsius(this double fahrenheit)
            => fahrenheit * FahrenheitCelsiusConst;
    }
}
