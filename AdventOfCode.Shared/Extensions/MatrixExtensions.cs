namespace AdventOfCode.Shared.Extensions
{
    public static class MatrixExtensions
    {
        public static double[] GaussianElimination(this double[,] matrix)
        {
            if (matrix.GetLength(0) - matrix.GetLength(1) != 1)
            {
                throw new ArgumentException("matrix needs 1 column more then rows to solve");
            }

            matrix = MakeEchelon(matrix);
            matrix = MakeUniform(matrix);

            return BackSubstitute(matrix);
        }

        private static double[] BackSubstitute(double[,] matrix)
        {
            var result = new double[matrix.GetLength(1)];
            for (int j = matrix.GetLength(1) - 1; j >= 0; j--)
            {
                result[j] = matrix[matrix.GetLength(0) - 1, j];
                if (j > 0 && Math.Abs(result[j]) > double.Epsilon)
                {
                    for (int row = j - 1; row >= 0; row--)
                    {
                        var value = matrix[j, row] * result[j];
                        matrix[matrix.GetLength(0) - 1, row] -= value;
                        matrix[j, row] = 0;
                    }
                }
            }            
            return result;
        }

        private static double[,] MakeUniform(double[,] matrix) 
        {
            for (int j = 0; j < matrix.GetLength(1); j++)
            {
                var factor = matrix[j,j];
                if (Math.Abs(factor) > double.Epsilon)
                {
                    for (int i = j; i < matrix.GetLength(0); i++)
                    {
                        matrix[i, j] /= factor;
                    }
                }
            }
            return matrix;
        }

        private static double[,] MakeEchelon(double[,] matrix)
        {
            for (int j = 0; j < matrix.GetLength(1) - 1; j++)
            {
                matrix = Reorder(matrix, j);
                var value = matrix[j, j];
                if (Math.Abs(value) > double.Epsilon)
                {
                    for (int row = j + 1; row < matrix.GetLength(1); row++)
                    {
                        var factor = matrix[j, row] / value;
                        for (int i = j; i < matrix.GetLength(0); i++)
                        {
                            var newValue = matrix[i, j] * factor;
                            if (Math.Abs(newValue - (long)newValue) < double.Epsilon)
                            {
                                newValue = (long)newValue;
                            }
                            matrix[i, row] -= newValue;
                        }
                    }
                }
            }
            return matrix;
        }

        private static double[,] Reorder(double[,] matrix, int coefficient)
        {
            var result = new double[matrix.GetLength(0), matrix.GetLength(1)];
            List<double[]> empty = [];

            for (int j = 0; j < matrix.GetLength(1); j++)
            {
                var row = new double[matrix.GetLength(0)];
                if (j >= coefficient && matrix[coefficient, j] == 0)
                {
                    empty.Add(row);
                }
                for (int i = 0; i < matrix.GetLength(0); i++)
                {
                    if (j < coefficient)
                    {
                        result[i, j] = matrix[i, j];
                    }
                    else if (matrix[coefficient, j] == 0)
                    {
                        row[i] = matrix[i, j];
                    }
                    else
                    {
                        result[i, j - empty.Count] = matrix[i, j];
                    }
                }
            }

            for (int j = 0; j < empty.Count; j++) 
            {                
                var row = result.GetLength(1) - empty.Count + j;
                for (int i = 0; i < result.GetLength(0); i++)
                {
                    result[i, row] = empty[j][i];
                }
            }

            return result;
        }

        private static void Print(double[,] matrix)
        {
            System.Diagnostics.Debug.WriteLine(string.Empty);
            for (int j = 0; j < matrix.GetLength(1); j++)
            {
                for (int i = 0; i < matrix.GetLength(0); i++)
                {
                    System.Diagnostics.Debug.Write($"{Math.Round(matrix[i, j], 2),6} ");

                }
                System.Diagnostics.Debug.WriteLine(string.Empty);
            }
        }
    }
}
