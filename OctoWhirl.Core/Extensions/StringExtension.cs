namespace OctoWhirl.Core.Extensions
{
    public static class StringExtension
    {
        public static DateTime ToDateTime(this string value)
        {
            if (DateTime.TryParse(value, out var date)) 
                return date;

            throw new ArgumentException($"{value} is non valid DateTime");
        }
    }
}
