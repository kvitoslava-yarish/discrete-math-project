using System;

namespace MPMAlgorithm
{
    public class MatrixGraph
    {
        public MatrixGraph(int vertexNumber, double possibility, int maxWeight)
        {
            var random = new Random();
            var matrix = new int[vertexNumber, vertexNumber];
            for (var i = 0; i < vertexNumber; i++)
            {
                for (var j = 0; j < vertexNumber; j++)
                {
                    var randomNum = random.NextDouble();
                    if (randomNum > possibility || i == j)
                    {
                        matrix[i, j] = 0;
                        continue;
                    }
                    var weight = random.Next(1, maxWeight);
                    matrix[i, j] = weight;
                }
            }
        }
    }
}