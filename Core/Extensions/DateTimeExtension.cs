namespace OctoWhirl.Core.Extensions
{
    public static class DateTimeExtension
    {
        public static long ToUnixTimestamp(this DateTime dateTime) => ((DateTimeOffset)dateTime).ToUnixTimeSeconds();
    }
}