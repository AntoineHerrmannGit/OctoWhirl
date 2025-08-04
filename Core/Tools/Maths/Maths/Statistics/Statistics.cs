using OctoWhirl.Core.Models.Exceptions;

namespace OctoWhirl.Maths.Statistics
{
    public static class Statistics
    {
        public static double Correlation(IEnumerable<double> serie1, IEnumerable<double> serie2, double lambda = 0)
        {
            if (!serie1.Any())
                throw new EmptyEnumerableException(nameof(serie1));
            if (!serie2.Any())
                throw new EmptyEnumerableException(nameof(serie2));

            var enumerator1 = serie1.GetEnumerator();
            var enumerator2 = serie2.GetEnumerator();

            if (!enumerator1.MoveNext() || !enumerator2.MoveNext())
                throw new EmptyEnumerableException("One of the series is empty");

            double sum1 = enumerator1.Current;
            double sum2 = enumerator2.Current;
            double sum1Squared = enumerator1.Current * enumerator1.Current;
            double sum2Squared = enumerator2.Current * enumerator2.Current;
            double sumProduct = enumerator1.Current * enumerator2.Current;
            int count = 1;

            while (enumerator1.MoveNext() && enumerator2.MoveNext())
            {
                sum1 += enumerator1.Current;
                sum2 += enumerator2.Current;
                sum1Squared += enumerator1.Current * enumerator1.Current;
                sum2Squared += enumerator2.Current * enumerator2.Current;
                sumProduct += enumerator1.Current * enumerator2.Current;
                count++;
            }

            if (enumerator1.MoveNext())
                throw new IndexOutOfRangeException(nameof(serie1));
            if (enumerator2.MoveNext())
                throw new IndexOutOfRangeException(nameof(serie2));

            double mean1 = sum1 / count;
            double mean2 = sum2 / count;
            double var1 = sum1Squared / count - mean1 * mean1;
            double var2 = sum2Squared / count - mean2 * mean2;
            double crossedTerms = sumProduct / count;

            if (var1 == 0 && var2 == 0)
                return 1.0;
            else if (var1 == 0  || var2 == 0)
                return 0.0;

            double numerator = crossedTerms - mean1 * mean2;
            double denominator = Math.Sqrt(var1 * var2);
            double correlation = numerator / denominator;

            if (lambda != 0)
            {
                double sgn = Math.Sign(lambda);
                correlation += sgn * (1 - sgn * correlation) * Math.Abs(lambda);
            }

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
