using OctoWhirl.Core.Tools.Maths.Generators.Interfaces;
using OctoWhirl.Core.Tools.Maths.Maths.Statistics;

namespace OctoWhirl.Core.Tools.Maths.Generators
{
    public class MultiVariableGenerator : IMultiGenerator<double>
    {
        private readonly double[,] _correlations;
        private readonly double[,] _choleskyMatrix;
        private readonly int _dimension;
        private readonly ISimpleGenerator<double> _generator;

        public MultiVariableGenerator(double[,] correlations, ISimpleGenerator<double>? generator = null)
        {
            CheckMatrix(correlations);

            _correlations = correlations;
            _choleskyMatrix = Cholesky.CholeskyDecomposition(correlations);
            _dimension = _correlations.GetLength(0);

            _generator = generator ?? new GaussianGenerator(mean: 0, sigma: 1);
        }

        public double[] GetNext()
        {
            double[] variables = new double[_dimension];
            for (int i = 0; i < _dimension; i++)
                variables[i] = _generator.GetNext();

            double[] result = new double[_dimension];
            for (int i = 0; i < _dimension; i++)
            {
                result[i] = 0;
                for (int j = 0; j <= i; j++)
                    result[i] += _choleskyMatrix[i, j] * variables[j];
            }

            return result;
        }

        public void Reset()
        {
            _generator.Reset();
        }

        #region Private Methods
        private static void CheckMatrix(double[,] matrix)
        {
            if (matrix.GetLength(0) != matrix.GetLength(1))
                throw new ArgumentOutOfRangeException($"{nameof(matrix)} must be squared.");

            int size = matrix.GetLength(0);
            for (int j = 0; j < size; j++)
            {
                for (int k = 0; k < size; k++)
                {
                    if (j == k && matrix[j, k] != 1)
                        throw new ArgumentException($"{nameof(matrix)} must be of the form '1 + L' with L a matrix with zeros on the diagonal.");
                    if (j != k && matrix[j, k] != matrix[k, j])
                        throw new ArgumentException($"{nameof(matrix)} must be symmetric.");
                    if (k != j && matrix[j, k] < -1 || matrix[j, k] > 1)
                        throw new ArgumentException($"{nameof(matrix)} must be of values in [-1, 1].");
                }
            }
        }
        #endregion Private Methods
    }
}
