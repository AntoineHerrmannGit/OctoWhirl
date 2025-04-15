namespace OctoWhirl.Core.Extensions
{
    public static class SystemExtensions
    {
        public static bool IsNullOrEmpty(this object obj)
        {
            if (obj == null)
                return true;

            if (obj is string str && string.IsNullOrEmpty(str))
                return true;

            if (obj is IEnumerable<object> collection && collection.GetEnumerator().MoveNext())
                return false;

            return false;
        }
    }
}
