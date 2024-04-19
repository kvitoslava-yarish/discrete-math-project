using System;

namespace MPMAlgorithm
{
    public class MatrixGraph
    {
        private int[,] _matrix;
        private int _vertexNumber;
        private Random _random;
        private int _maxWeight;

        public MatrixGraph(int vertexNumber, double possibility, int maxWeight)
        {
            _vertexNumber = vertexNumber;
            _random = new Random();
            _matrix = new int[vertexNumber, vertexNumber];
            _maxWeight = maxWeight;
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

        public void PrintGraph()
        {
            for (var i = 0; i < _vertexNumber; i++)
            {
                Console.Write("[ ");
                for (var j = 0; j < _vertexNumber; j++)
                {
                    Console.Write($"{_matrix[i, j]} ");
                }
                Console.Write("]\n");
            }
        }

        public int EdgeNumber()
        {
            var count = 0;
            for (var i = 0; i < _vertexNumber; i++)
            {
                for (var j = 0; j < _vertexNumber; j++)
                {
                    if (_matrix[i, j] != 0)
                    {
                        count += 1;
                    }
                }
            }
            return count;
        }
    } 
}