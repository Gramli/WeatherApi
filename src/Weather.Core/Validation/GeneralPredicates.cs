namespace Weather.Core.Validation
{
    internal static class GeneralPredicates
    {
        internal static Predicate<string> isValidString = m => !string.IsNullOrWhiteSpace(m);
        internal static Predicate<double> isValidTemperature = m => m < 60 && m > -90;
    }
}
