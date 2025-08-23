namespace OctoWhirl.Core.Tools.Maths.Maths.Statistics
{
    public static class Cholesky
    {
        /// <summary>
        /// Returns an Inferior diagonal matrix L from a semi-positive matrix M such that
        /// M = L * L^T, with M and L real matrices.
        /// </summary>
        public static double[,] CholeskyDecomposition(double[,] matrix)
        {
            int size = matrix.GetLength(0);
            double[,] choleskyMatrix = new double[size, size];

            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j <= i; j++)
                {
                    double sum = 0;
                    for (int k = 0; k < j; k++)
                        sum += choleskyMatrix[i, k] * choleskyMatrix[j, k];

                    double element = matrix[i, j] - sum;
                    if (i == j)
                    {
                        if (element < 0)
                            throw new ArgumentException($"{nameof(matrix)} n'est pas définie positive.");
                        choleskyMatrix[i, j] = Math.Sqrt(element);
                    }
                    else
                        choleskyMatrix[i, j] = element / choleskyMatrix[j, j];
                }
            }

            return choleskyMatrix;
        }
    }
}
