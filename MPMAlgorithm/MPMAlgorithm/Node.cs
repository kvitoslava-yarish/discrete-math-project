using System;
using System.Collections.Generic;
namespace MPMAlgorithm
{
    public class Node // Подання графу у вигляді списків суміжності
    {
        private Dictionary<int, List<Tuple<int, int>>> _adjacencyList;

        public Node()
        {
            _adjacencyList = new Dictionary<int, List<Tuple<int, int>>>();
        }

        private void AddVertex(int vertex)
        {
            if (!_adjacencyList.ContainsKey(vertex))
            {
                _adjacencyList[vertex] = new List<Tuple<int, int>>();
            }
        }

        private void AddEdge(int start, int destination, int weight)
        {
            if (!_adjacencyList.ContainsKey(start))
            {
                AddVertex(start);
            }

            if (!_adjacencyList.ContainsKey(destination))
            {
                AddVertex(destination);
            }
            _adjacencyList[start].Add(new Tuple<int, int>(destination, weight));
        }

        public void GenerationGraph(double possibility, int vertexNumber, int maxWeight)
        {
            var random = new Random();
            for (var i = 0; i < vertexNumber; i++)
            {
                for (var j = 0; j < vertexNumber; j++)
                {
                    var randomNum = random.NextDouble();
                    if (randomNum > possibility || i == j) continue;
                    var weight = random.Next(1, maxWeight);
                    AddEdge(i, j, weight);
                }
            }
        }
    }
}