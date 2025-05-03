using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OctoWhirl.Core.Extensions
{
    public static class EnumerableExtensions
    {
        public static bool IsNotEmpty<T>(this IEnumerable<T> obj)
        {
            return obj.Any();
        }

        public static bool IsEmpty<T>(this IEnumerable<T> obj)
        {
            return !obj.Any();
        }
        public static bool IsNullOrEmpty<T>(this IEnumerable<T> obj)
        {
            return obj.IsNull() || !obj.Any();
        }

    }
}
