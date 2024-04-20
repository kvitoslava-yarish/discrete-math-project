using System;
using System.Diagnostics;


namespace MPMAlgorithm
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            var graph1 = new AdjacencyList();
            graph1.GenerationGraph(0.5, 200, 100);
            var algorithm = new MPM(graph1, 0, 159);
            graph1.PrintGraph();
            var sw = new Stopwatch();
            sw.Start();
            var t = algorithm.MaxFlow();
            sw.Stop();
            Console.WriteLine($"Elapsed time: {sw.Elapsed}");
            Console.WriteLine(t);
            string[] result = {$"{sw.Elapsed}, {200}, {100}, {0.5}" };
            FileWork.WriteToCSV(
                "C:\\Users\\pavlo\\RiderProjects\\discrete-math-project\\MPMAlgorithm\\MPMAlgorithm\\output.csv",
                result);

        }
        }
    }

