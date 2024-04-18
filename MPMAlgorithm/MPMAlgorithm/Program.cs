using System;

namespace MPMAlgorithm
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            var graph1 = new MatrixGraph(20, 0.5, 20);
            graph1.PrintGraph();
        }
    }
}
