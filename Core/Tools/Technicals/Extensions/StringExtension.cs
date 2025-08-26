namespace OctoWhirl.Core.Tools.Technicals.Extensions
{
    public static class StringExtension
    {
        public static string Stringify(this IEnumerable<object> str, string separator = ", ")
        {
            return string.Join(separator, str);
        }

        public static DateTime ToDateTime(this string value)
        {
            if (DateTime.TryParse(value, out var date))
                return date;

            throw new ArgumentException($"{value} is non valid DateTime");
        }
    }
}
