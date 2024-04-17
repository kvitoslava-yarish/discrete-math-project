using System;

namespace MPMAlgorithm
{
    public class MatrixGraph
    {
        private int[,] _matrix;
        private int _vertexNumber;
        private Random _random;

        public MatrixGraph(int vertexNumber, double possibility, int maxWeight)
        {
            _vertexNumber = vertexNumber;
            _random = new Random();
            _matrix = new int[vertexNumber, vertexNumber];
            GenerateGraph(possibility, maxWeight);
        }

        private void GenerateGraph(double possibility, int maxWeight)
        {
            for (var i = 0; i < _vertexNumber; i++)
            {
                for (var j = 0; j < _vertexNumber; j++)
                {
                    var randomNum = _random.NextDouble();
                    if (randomNum > possibility || i == j)
                    {
                        _matrix[i, j] = 0;
                        continue;
                    }
                    var weight = _random.Next(1, maxWeight);
                    _matrix[i, j] = weight;
                }
            }
        }

        public void EditorEdges(int start, int destination, int flow)
        {
            _matrix[start, destination] -= flow;
        }
    }
}