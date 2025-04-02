using System;
using System.Collections.Generic;

namespace OctoWhirl.Core.Models.Common
{
    public class TimeSerie<T> : SortedDictionary<DateTime, T>
    {
        #region Accessors
        public List<DateTime> Dates => Keys.ToList();
        public List<T> Values => base.Values.ToList();

        public DateTime LastDate => Dates.IsEmpty() ? null : Dates.Last();
        public T LastValue => Values.IsEmpty() ? null : Values.Last();
        #endregion Accessors

        public TimeSerie<T>(IEnumerable<KeyValuePair<DateTime, T>> enumerable):
            base(enumerable)
        {
        }

        public TimeSerie<T>(IEnumerable<DateTime> dates, IEnumerable<T> values)
        {
            if (dates == null)
                throw new ArgumentNullException(nameof(dates));
            if (values == null)
                throw new ArgumentNullException(nameof(values));
            if (dates.IsEmpty() && values.IsEmpty())
                base(); 
            if (dates.IsEmpty() && !values.IsEmpty()) || (!dates.IsEmpty() && values.IsEmpty())
                throw new ArgumentException("Dates and Values must have the same length or be both empty.");
            
            Dictionary<KeyValuePair<DateTime, T>> kvp = new Dictionary<KeyValuePair<DateTime, T>>();
            using (dateEnumerator = dates.GetEnumerator())
            {
                using (valueEnumerator = values.GetEnumerator())
                {
                    kvp.Add(dateEnumerator.Current, valueEnumerator.Current);
                    while (dateEnumerator.MoveNext() && valueEnumerator.MoveNext())
                        kvp.Add(dateEnumerator.Current, valueEnumerator.Current);
                    
                    if (dateEnumerator.MoveNext() || valueEnumerator.MoveNext())
                        throw new ArgumentException("Dates and Values must have the same length.");
                }
            }

            base(kvp);
        }
    }
}