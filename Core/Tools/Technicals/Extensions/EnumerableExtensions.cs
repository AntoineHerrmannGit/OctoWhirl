using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Technicals.Extensions;
using OctoWhirl.Core.Maths;

namespace OctoWhirl.Core.Extensions
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

        
        public static double Mean(this IEnumerable<double> @this)
        {
            return Statistics.Mean(@this);
        }

        public static double Variance(this IEnumerable<double> @this)
        {
            return Statistics.Variance(@this);
        }

        public static double StdDev(this IEnumerable<double> @this)
        {
            return Statistics.StdDev(@this);
        }

        public static double Covariance(this IEnumerable<double> @this, IEnumerable<double> other)
        {
            return Statistics.Covariance(@this, other);
        }

        public static double Correlation(this IEnumerable<double> @this, IEnumerable<double> other, double lambda = 0)
        {
            return Statistics.Correlation(@this, other, lambda);
        }

        public static double Moment(this IEnumerable<double> @this, int order)
        {
            return Statistics.Moment(@this, order);
        }

        public static double Skew(this IEnumerable<double> @this)
        {
            return Statistics.Skew(@this);
        }

        public static double Kurtosis(this IEnumerable<double> @this)
        {
            return Statistics.Kurtosis(@this);
        }
    }
}
