using System;
using System.Collections.Generic;
using System.Linq;

namespace MPMAlgorithm
{
    public class AdjacencyList // Подання графу у вигляді списків суміжності
    {
        public Dictionary<int, List<int[]>> _adjacencyList = new Dictionary<int, List<int[]>>();

        private void AddVertex(int vertex)
        {
            if (!_adjacencyList.ContainsKey(vertex))
            {
                _adjacencyList[vertex] = new List<int[]>();
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
            _adjacencyList[start].Add(new int[] {destination, weight});
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

        public void EditorEdge(int start, int destination, int flow, bool forward)
        {
            if (!forward)
            {
                (start, destination) = (destination, start);
            }
            foreach (var edge in _adjacencyList[start].Where(edge => edge[0] == destination))
            {
                edge[1] -= flow;
            }
        }
        public void PrintGraph()
        {
            foreach (var vertex in _adjacencyList)
            {
                Console.Write($"Node {vertex.Key}: ");
                foreach (var edge in vertex.Value)
                {
                    Console.Write($"({edge[0]}:{edge[1]});");
                }
                Console.WriteLine();
            }
        }

        public List<int> GetKeys()
        {
            return _adjacencyList.Keys.ToList();
        }
        public List<List<int[]>> GetVertexes()
        {
            return _adjacencyList.Values.ToList();
        }
        
        public List<int[]> GetValue(int index)
        {
            return _adjacencyList[index];
        }
    }
}