using System;
using System.Collections.Generic;
using System.Linq;

namespace MPMAlgorithm
{
    public class AdjacencyListGraph // Подання графу у вигляді списків суміжності
    {
        public Dictionary<int, List<int[]>> AdjacencyList { get; private set; } = new Dictionary<int, List<int[]>>();

        private void AddVertex(int vertex)
        {
            if (!AdjacencyList.ContainsKey(vertex))
            {
                AdjacencyList[vertex] = new List<int[]>();
            }
        }
        private void AddEdge(int start, int destination, int weight)
        {
            if (!AdjacencyList.ContainsKey(start))
            {
                AddVertex(start);
            }

            if (!AdjacencyList.ContainsKey(destination))
            {
                AddVertex(destination);
            }
            AdjacencyList[start].Add(new int[] {destination, weight});
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
            foreach (var edge in AdjacencyList[start].Where(edge => edge[0] == destination))
            {
                edge[1] -= flow;
            }
        }
        public void PrintGraph()
        {
            foreach (var vertex in AdjacencyList)
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
            return AdjacencyList.Keys.ToList();
        }
        public Array GetVertexes()
        {
            return AdjacencyList.Values.ToArray();
        }
        
        public List<int[]> GetValue(int index)
        {
            return AdjacencyList[index];
        }
    }
}