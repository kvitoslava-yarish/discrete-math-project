
﻿using System;

namespace MPMAlgorithm
{
    public class MatrixGraph
    {
        public int[,] Matrix { get; set; } 
        private int _vertexNumber;
        private Random _random;
        private int _maxWeight;
        

        public void GenerateGraph(int vertexNumber, double possibility, int maxWeight)
        {
            Matrix = new int[vertexNumber, vertexNumber];
            for (var i = 0; i < _vertexNumber; i++)
            {
                for (var j = 0; j < _vertexNumber; j++)
                {
                    var randomNum = _random.NextDouble();
                    if (randomNum > possibility || i == j)
                    {
                        Matrix[i, j] = 0;
                        continue;
                    }
                    var weight = _random.Next(1, maxWeight);
                    Matrix[i, j] = weight;
                }
            }
        }

        public void EditEdges(int start, int destination, int flow, bool forward)
        {
            
            if (!forward)
            {
                (start, destination) = (destination, start);
            }
            Matrix[start, destination] -= flow;
        }

        public void PrintGraph()
        {
            for (var i = 0; i < _vertexNumber; i++)
            {
                Console.Write("[ ");
                for (var j = 0; j < _vertexNumber; j++)
                {
                    Console.Write($"{Matrix[i, j]} ");
                }
                Console.Write("]\n");
            }
        }
    }
}
