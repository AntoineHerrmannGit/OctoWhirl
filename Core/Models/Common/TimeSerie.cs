namespace OctoWhirl.Core.Models.Common
{
    public class TimeSerie<T> : SortedDictionary<DateTime, T>
    {
        #region Accessors
        public List<DateTime> Dates => Keys.ToList();
        public new List<T> Values => base.Values.ToList();

        public DateTime LastDate => Dates.Any() ? Dates.Last() : throw new ArgumentOutOfRangeException(nameof(Dates));
        public T LastValue => Values.Any() ? Values.Last() : throw new ArgumentOutOfRangeException(nameof(Values));
        #endregion Accessors

        public TimeSerie(IEnumerable<KeyValuePair<DateTime, T>> enumerable) : 
            base(enumerable.ToDictionary())
        {
        }

        public TimeSerie(IEnumerable<DateTime> dates, IEnumerable<T> values)
            : base(Build(dates, values))
        {
        }

        private static IDictionary<DateTime, T> Build(IEnumerable<DateTime> dates, IEnumerable<T> values)
        {
            if (dates == null)
                throw new ArgumentNullException(nameof(dates));
            if (values == null)
                throw new ArgumentNullException(nameof(values));
            if (!dates.Any() && !values.Any())
                return new Dictionary<DateTime, T>();
            if ((!dates.Any() && values.Any()) || (dates.Any() && !values.Any()))
                throw new ArgumentException("Dates and Values must have the same length or be both empty.");


            Dictionary<DateTime, T> kvp = new Dictionary<DateTime, T>();
            using (IEnumerator<DateTime> dateEnumerator = dates.GetEnumerator())
            {
                using (IEnumerator<T> valueEnumerator = values.GetEnumerator())
                {
                    kvp.Add(dateEnumerator.Current, valueEnumerator.Current);
                    while (dateEnumerator.MoveNext() && valueEnumerator.MoveNext())
                        kvp.Add(dateEnumerator.Current, valueEnumerator.Current);

                    if (dateEnumerator.MoveNext() || valueEnumerator.MoveNext())
                        throw new ArgumentException("Dates and Values must have the same length.");
                }
            }

            return kvp;
        }
    }
}