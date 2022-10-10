namespace Weather.Domain.Extensions
{
    public static class EnumerableExtensions
    {
        public static void ForEach<T>(this IEnumerable<T> values, Action<T> action)
        {
            foreach(var item in values)
            {
                action(item);
            }
        }
    }
}
