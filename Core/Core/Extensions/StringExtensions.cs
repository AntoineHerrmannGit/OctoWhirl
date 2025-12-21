namespace Core.Technicals.Extensions
{
    public static class StringExtensions
    {
        public static string Stringify<T>(this IEnumerable<T> @this, string separator = ", ")
            where T : class
            => string.Join(separator, @this);

        public static string ToLowerString<T>(this T @this)
            => @this.ToString().ToLower();

        public static DateTime? ToDate(this string @this)
            => DateTime.TryParse(@this, out var date) ? date : null;
    }
}
