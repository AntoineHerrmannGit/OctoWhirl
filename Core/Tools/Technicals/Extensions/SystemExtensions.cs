namespace Technicals.Extensions
{
    public static class SystemExtensions
    {
        public static bool IsNull(this object obj)
        {
            return obj == null;
        }

        public static bool IsNotNull(this object obj)
        {
            return obj != null;
        }

        public static bool IsDefault<T>(this T obj)
        {
            return obj.Equals(default(T));
        }

        public static bool IsNullOrDefault<T>(this T obj)
        {
            return obj == null || obj.Equals(default(T));
        }
    }
}
