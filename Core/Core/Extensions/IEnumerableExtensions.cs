namespace Core.Technicals.Extensions
{
    public static class IEnumerableExtensions
    {
        public static bool IsEmpty<T>(this IEnumerable<T> @this)
            => !@this.Any();

        public static bool IsNullOrEmpty<T>(this IEnumerable<T> @this)
            => @this is null || !@this.Any();

        public static bool None<T>(this IEnumerable<T> @this, Func<T, bool> selector)
            => !@this.Any(selector);

        public static void ForEach<T>(this IEnumerable<T> @this, Action<T> action)
        {
            foreach (T @item in @this)
                action(@item);
        }

        public static IEnumerable<T> Flatten<T>(this IEnumerable<IEnumerable<T>> @this)
            => @this.SelectMany(x => x);
    }
}
