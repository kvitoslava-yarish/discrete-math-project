using System;
using System.Diagnostics.CodeAnalysis;

namespace MPMAlgorithm
{
    internal class Program
    {
        [SuppressMessage("ReSharper.DPA", "DPA0003: Excessive memory allocations in LOH", MessageId = "type: System.Int32[]; size: 2048MB")]
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
