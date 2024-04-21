using System;
using System.Diagnostics;
using System.Text;


namespace MPMAlgorithm
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            var graph1 = new AdjacencyListGraph();
            var vertexNum = 20;
            var possibility = 0.3;
            var maxWeight = 100;
            var result = new StringBuilder();
            var sw = new Stopwatch();
            Console.WriteLine($"Elapsed time: {sw.Elapsed}");
            
            for (int i = 0; i < 5; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    for (int n = 1; n < 21; n++)
                    {
                        graph1.GenerationGraph(possibility, vertexNum, maxWeight);
                        var algorithm = new MPM(graph1, 0, vertexNum - 1);
 
                        sw.Start();
                        var t = algorithm.MaxFlow();
                        sw.Stop();
                        result.Append($"{n}, {vertexNum}, {possibility.ToString().Replace(",", ".")}, {sw.Elapsed}\n");
                    }

                    possibility += 0.1;
                }

                possibility = 0.3;
                vertexNum += 20;
            }
            FileWork.WriteToCSV(
                "C:\\Users\\pavlo\\RiderProjects\\discrete-math-project\\MPMAlgorithm\\MPMAlgorithm\\output.csv",
                result.ToString());

        }
        }
    }

