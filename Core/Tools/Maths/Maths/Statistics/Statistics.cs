using OctoWhirl.Core.Models.Exceptions;

namespace OctoWhirl.Maths.Statistics
{
    public static class Statistics
    {
        public static double Correlation(IEnumerable<double> serie1, IEnumerable<double> serie2)
        {
            if (!serie1.Any())
                throw new EmptyEnumerableException(nameof(serie1));
            if (!serie2.Any())
                throw new EmptyEnumerableException(nameof(serie1));

            var enumerator1 = serie1.GetEnumerator();
            var enumerator2 = serie2.GetEnumerator();

            double mean1 = enumerator1.Current;
            double mean2 = enumerator2.Current;
            double var1 = enumerator1.Current * enumerator1.Current;
            double var2 = enumerator2.Current * enumerator2.Current;
            double crossedTerms = enumerator1.Current * enumerator2.Current;
            while (enumerator1.MoveNext() && enumerator2.MoveNext())
            {
                mean1 += enumerator1.Current;
                mean2 += enumerator2.Current;
                var1 += enumerator1.Current * enumerator1.Current;
                var2 += enumerator2.Current * enumerator2.Current;
                crossedTerms += enumerator1.Current * enumerator2.Current;
            }

            if (enumerator1.MoveNext())
                throw new IndexOutOfRangeException(nameof(serie1));
            if (enumerator2.MoveNext())
                throw new IndexOutOfRangeException(nameof(serie2));

            double numerator = crossedTerms - mean1 * mean2;
            double denominator = Math.Sqrt((var1 - mean1 * mean1) * (var2 - mean2 * mean2));
            if (denominator == 0)
                throw new DivideByZeroException(nameof(denominator));

            double correlation = numerator / denominator;
            return correlation;
        }

        public static double StdDev(IEnumerable<double> serie) => Math.Sqrt(Variance(serie));

        public static double Variance(IEnumerable<double> serie)
        {
            if (!serie.Any())
                throw new EmptyEnumerableException(nameof(serie));

            double mean = 0;
            double square = 0;
            int count = 0;
            foreach(double value in serie)
            {
                mean += value;
                square += value * value; 
                count++;
            }

            mean /= count;
            square /= count;
            return square - mean * mean;
        }

        public static double Mean(IEnumerable<double> serie) => serie.Average();

        public static double Moment(IEnumerable<double> serie, int order)
        {
            if(!serie.Any())
                throw new EmptyEnumerableException(nameof(serie));

            double result;
            switch (order)
            {
                case 0:
                    throw new ArgumentException(nameof(order));
                case 1:
                    result = Mean(serie);
                    break;
                case 2:
                    result = Variance(serie);
                    break;
                default:
                    double mean = Mean(serie);
                    double moment = 0;
                    double var = 0;
                    int count = 0;
                    foreach (double value in serie)
                    {
                        moment += Math.Pow(value - mean, order);
                        var += value * value;
                        count++;
                    }
                    double sigma = Math.Pow(var / count - mean * mean, order / 2);

                    result = moment / sigma;
                    break;
            }

            return result;
        }

        public static double Skew(IEnumerable<double> serie) => Moment(serie, 3);

        public static double Kurtosis(IEnumerable<double> serie) => Moment(serie, 4);
    }
}
