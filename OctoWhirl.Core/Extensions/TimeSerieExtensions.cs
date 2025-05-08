using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OctoWhirl.Core.Models.Technicals;

namespace OctoWhirl.Core.Extensions
{
    public static class TimeSerieExtensions
    {
        public static TimeSerie<T> Clear<T>(this TimeSerie<T> serie)
        {
            return new TimeSerie<T>();
        }

        public static TimeSerie<T> ToTimeSerie<T>(this IEnumerable<KeyValuePair<DateTime, T>> enumerable)
        {
            if (enumerable == null)
                throw new ArgumentNullException(nameof(enumerable));
            return new TimeSerie<T>(enumerable);
        }

        public static IEnumerable<KeyValuePair<DateTime, IEnumerable<T>>> Rolling<T>(this TimeSerie<T> serie, int windowSize)
        {
            if (windowSize <= 0)
                throw new ArgumentOutOfRangeException(nameof(windowSize), "Window size must be greater than 0.");

            for (int i = 0; i < serie.Count - windowSize; i++)
            {
                yield return new KeyValuePair<DateTime, IEnumerable<T>>(
                    serie.Dates[i + windowSize],
                    serie.Values.Skip(i).Take(windowSize)
                );
            }
        }

        public static IEnumerable<KeyValuePair<DateTime, TOutput>> Rolling<TInput, TOutput>(this TimeSerie<TInput> serie, int windowSize, Func<IEnumerable<TInput>, TOutput> func)
        {
            if (windowSize <= 0)
                throw new ArgumentOutOfRangeException(nameof(windowSize), "Window size must be greater than 0.");

            for (int i = 0; i < serie.Count - windowSize; i++)
            {
                yield return new KeyValuePair<DateTime, TOutput>(
                    serie.Dates[i + windowSize],
                    func(serie.Values.Skip(i).Take(windowSize))
                );
            }
        }
    }
}
