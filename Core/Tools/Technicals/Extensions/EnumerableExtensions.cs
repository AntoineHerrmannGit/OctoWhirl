namespace OctoWhirl.Core.Tools.Technicals.Extensions
{
    public static class EnumerableExtensions
    {
        public static bool IsNotEmpty<T>(this IEnumerable<T> @this)
        {
            return @this.Any();
        }

        public static bool IsEmpty<T>(this IEnumerable<T> @this)
        {
            return !@this.Any();
        }

        public static bool IsNullOrEmpty<T>(this IEnumerable<T> @this)
        {
            return @this.IsNull() || !@this.Any();
        }

        public static double Mean(this IEnumerable<double> @this, Func<double, double> selector = null)
        {
            if (!@this.Any())
                throw new ArgumentOutOfRangeException($"Enumerable must have at least one element.");

            if (selector.IsNull())
                selector = new Func<double, double>(x => x);

            double mean = 0;
            int count = 0;
            foreach (double element in @this)
            {
                mean += selector(element);
                count++;
            }
            return mean / count;
        }

        public static double Var(this IEnumerable<double> @this, Func<double, double> selector = null)
        {
            if (!@this.Any())
                throw new ArgumentOutOfRangeException($"Enumerable must have at least one element.");

            if (selector.IsNull())
                selector = new Func<double, double>(x => x);

            double stddev = 0;
            double mean = 0;
            int count = 0;
            foreach (double element in @this)
            {
                double stepElement = selector(element);
                stddev += stepElement * stepElement;
                mean += stepElement;
                count++;
            }
            mean /= count;
            return (stddev - mean * mean) / count;
        }

        public static double StdDev(this IEnumerable<double> @this, Func<double, double> selector = null)
        {
            return Math.Sqrt(@this.Var(selector));
        }

        public static double CoVar(this IEnumerable<double> @this, IEnumerable<double> @other, Func<double, double> @thisSelector = null, Func<double, double> @otherSelector = null)
        {
            if (!@this.Any() || !other.Any())
                throw new ArgumentOutOfRangeException($"Enumerable must have at least one element.");

            bool isSizeChecked = false;
            if (@thisSelector.IsNull())
                @thisSelector = new Func<double, double>(x => x);
            else
            {
                if (@this.Count() != @other.Count())
                    throw new ArgumentOutOfRangeException($"Enumerables must have the same number of elements.");
                isSizeChecked = true;
            }

            if (@otherSelector.IsNull())
                @otherSelector = @thisSelector;
            else
                if (!isSizeChecked && @this.Count() != @other.Count())
                throw new ArgumentOutOfRangeException($"Enumerables must have the same number of elements.");

            double meanThis = 0;
            double meanOther = 0;
            double crossedTerm = 0;
            int count = 0;

            var @thisEnumerator = @this.GetEnumerator();
            var @otherEnumerator = @other.GetEnumerator();
            while (@thisEnumerator.MoveNext() && @otherEnumerator.MoveNext())
            {
                double @thisElement = @thisSelector(@thisEnumerator.Current);
                double @otherElement = @thisSelector(@otherEnumerator.Current);

                meanThis += @thisElement;
                meanOther += @otherElement;
                crossedTerm += @thisElement * @otherElement;
                count++;
            }

            if (@thisEnumerator.MoveNext() || @otherEnumerator.MoveNext())
                throw new ArgumentOutOfRangeException($"Enumerables must have the same number of elements.");

            return (crossedTerm - meanThis * meanOther) / count;
        }

        public static double Correl(this IEnumerable<double> @this, IEnumerable<double> @other, Func<double, double> @thisSelector = null, Func<double, double> @otherSelector = null)
        {
            if (!@this.Any() || !other.Any())
                throw new ArgumentOutOfRangeException($"Enumerable must have at least one element.");

            bool isSizeChecked = false;
            if (@thisSelector.IsNull())
                @thisSelector = new Func<double, double>(x => x);
            else
            {
                if (@this.Count() != @other.Count())
                    throw new ArgumentOutOfRangeException($"Enumerables must have the same number of elements.");
                isSizeChecked = true;
            }

            if (@otherSelector.IsNull())
                @otherSelector = @thisSelector;
            else
                if (!isSizeChecked && @this.Count() != @other.Count())
                throw new ArgumentOutOfRangeException($"Enumerables must have the same number of elements.");

            double meanThis = 0;
            double meanOther = 0;
            double stdDevThis = 0;
            double stdDevOther = 0;
            double crossedTerm = 0;
            int count = 0;

            var @thisEnumerator = @this.GetEnumerator();
            var @otherEnumerator = @other.GetEnumerator();
            while (@thisEnumerator.MoveNext() && @otherEnumerator.MoveNext())
            {
                double @thisElement = @thisSelector(@thisEnumerator.Current);
                double @otherElement = @thisSelector(@otherEnumerator.Current);

                meanThis += @thisElement;
                meanOther += @otherElement;
                stdDevThis += @thisElement * @thisElement;
                stdDevOther += @otherElement * @otherElement;
                crossedTerm += @thisElement * @otherElement;
                count++;
            }

            if (@thisEnumerator.MoveNext() || @otherEnumerator.MoveNext())
                throw new ArgumentOutOfRangeException($"Enumerables must have the same number of elements.");

            return (crossedTerm - meanThis * meanOther)
                / Math.Sqrt((stdDevThis - meanThis * meanThis) * (stdDevOther - meanOther * meanOther));
        }


    }
}
