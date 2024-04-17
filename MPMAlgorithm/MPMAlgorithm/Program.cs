using System;

namespace MPMAlgorithm
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            var graph1 = new AdjacencyList();
            graph1.GenerationGraph(0.5, 20, 20);
            Console.WriteLine('1');
        }
    }
}
