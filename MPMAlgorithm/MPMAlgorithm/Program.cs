using System;

namespace MPMAlgorithm
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            var graph1 = new AdjacencyList();
            graph1.GenerationGraph(0.5,10 , 20);
            var algorithm = new MPM(graph1, 0, 20);
            algorithm.Flow();
            Console.WriteLine(algorithm);
        }
    }
}
